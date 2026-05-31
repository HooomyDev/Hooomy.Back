using EFCore.BulkExtensions;
using Hooome.Domain;
using Hooome.Domain.Enums;
using Hooome.Persistance;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Text;

namespace Hooome.WebApi.Services;

public class DataSeeder
{
    public async Task SeedAllDataAsync(HooomeDbContext context)
    {
        try
        {
            var companies = await SeedCompaniesAsync(context);
            await SeedAddressesAsync(context);
            var requests = await SeedRequestsAsync(context);
            await SeedComplaintsAsync(context, companies, requests);
            await SeedPollsAsync(context);
            await SeedPollOptionsAsync(context);
            await SeedPollVotesAsync(context);
            await SeedWorksAsync(context);

            await context.SaveChangesAsync();

            Log.Information("All data has been added successfully");
        }
        catch (Exception ex)
        {
            Log.Error($"Error adding data: {ex.Message}");
            throw;
        }
    }

    private static async Task SeedAddressesAsync(HooomeDbContext context)
    {
        if (context.Addresses.Any())
            return;

        var filePath = Path.Combine(
            hostEnvironment.ContentRootPath, 
            "static", 
            "minsk_addresses.csv"
        );

        var lines = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);
        var addresses = new List<Address>();

        Console.WriteLine($"Start loading addresses");
        // Используем InvariantCulture для парсинга чисел с точкой
        var culture = System.Globalization.CultureInfo.InvariantCulture;

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var parts = line.Split(';');
            if (parts.Length >= 5) // Теперь нужно минимум 5 колонок
            {
                try
                {
                    var street = parts[2].Trim().Trim('"');
                    var houseNumber = parts[1].Trim().Trim('"');

                    // Парсим координаты с InvariantCulture
                    decimal? latitude = null;
                    decimal? longitude = null;

                    if (parts.Length > 3 && !string.IsNullOrWhiteSpace(parts[3]))
                    {
                        var latString = parts[3].Trim().Trim('"');
                        if (decimal.TryParse(latString, System.Globalization.NumberStyles.Any, culture, out decimal lat))
                        {
                            latitude = lat;
                        }
                        else
                        {
                            Console.WriteLine($"Не удалось распарсить широту: '{latString}'");
                        }
                    }

                    if (parts.Length > 4 && !string.IsNullOrWhiteSpace(parts[4]))
                    {
                        var lonString = parts[4].Trim().Trim('"');
                        if (decimal.TryParse(lonString, System.Globalization.NumberStyles.Any, culture, out decimal lon))
                        {
                            longitude = lon;
                        }
                        else
                        {
                            Console.WriteLine($"Не удалось распарсить долготу: '{lonString}'");
                        }
                    }

                    // Очищаем улицу от символов %
                    street = street.Replace('%', ' ').Replace("  ", " ").Trim();

                    if (houseNumber.Length > 50)
                        houseNumber = houseNumber[..50];

                    addresses.Add(new Address
                    {
                        Id = Guid.NewGuid(),
                        Street = street,
                        HouseNumber = houseNumber,
                        Latitude = latitude,
                        Longitude = longitude,
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обработки строки: {line}");
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            // Прогресс каждые 1000 строк
            if (addresses.Count % 1000 == 0 && addresses.Count > 0)
            {
                Console.WriteLine($"Обработано {addresses.Count} адресов...");
            }
        }

        // Массовая вставка (один запрос)
        await context.BulkInsertAsync(addresses);
        Console.WriteLine($"Saved {addresses.Count} addresses");
    }

    private static async Task<List<Company>> SeedCompaniesAsync(HooomeDbContext context)
    {
        // Проверяем, есть ли уже компании
        if (await context.Companies.AnyAsync())
            return await context.Companies.ToListAsync();

        var companies = new List<Company>();
        var random = new Random();

        // Получаем существующие адреса для привязки
        var addresses = await context.Addresses.ToListAsync();

        // Данные для компаний
        var companyData = new[]
        {
        new { Name = "ЖилКомСервис", Phone = "+7 (495) 123-45-67", Email = "info@zhilkom.ru", WorkingHours = "Пн-Пт 9:00-18:00" },
        new { Name = "Уютный Дом", Phone = "+7 (495) 234-56-78", Email = "contact@uytniydom.ru", WorkingHours = "Пн-Пт 8:00-20:00, Сб 10:00-16:00" },
        new { Name = "Городской Коммунальщик", Phone = "+7 (495) 345-67-89", Email = "office@gorcom.ru", WorkingHours = "Пн-Пт 9:00-17:30" },
        new { Name = "Комфорт Плюс", Phone = "+7 (495) 456-78-90", Email = "info@comfortplus.ru", WorkingHours = "Ежедневно 9:00-21:00" },
        new { Name = "Домоуправление №5", Phone = "+7 (495) 567-89-01", Email = "du5@mail.ru", WorkingHours = "Пн-Пт 8:30-17:00" },
        new { Name = "СтройКомСервис", Phone = "+7 (495) 678-90-12", Email = "info@stroikom.ru", WorkingHours = "Пн-Пт 9:00-18:00" },
        new { Name = "ТехЭксперт", Phone = "+7 (495) 789-01-23", Email = "support@techexpert.ru", WorkingHours = "Пн-Пт 9:00-18:00" },
        new { Name = "Энергия", Phone = "+7 (495) 890-12-34", Email = "info@energia.ru", WorkingHours = "Пн-Пт 9:00-17:30" },
        new { Name = "Зеленый Город", Phone = "+7 (495) 901-23-45", Email = "eco@greencity.ru", WorkingHours = "Пн-Пт 10:00-19:00" },
        new { Name = "Безопасный Дом", Phone = "+7 (495) 012-34-56", Email = "security@safedom.ru", WorkingHours = "Круглосуточно" }
    };

        foreach (var data in companyData)
        {
            // Находим подходящий адрес для компании (юридический)
            Address? legalAddress = null;

            // Пробуем найти адрес с таким же названием улицы как у компании (для разнообразия)
            if (addresses.Any())
            {
                var index = random.Next(addresses.Count);
                legalAddress = addresses[index];
            }

            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = data.Name,
                Phone = data.Phone,
                Email = data.Email,
                WorkingHours = data.WorkingHours,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(365, 1095)), // от 1 до 3 лет назад
                UpdatedAt = null,
                AddressId = legalAddress?.Id,
                Address = legalAddress,
                ServedAddresses = new List<Address>()
            };

            companies.Add(company);
        }

        // Сохраняем компании
        await context.Companies.AddRangeAsync(companies);
        await context.SaveChangesAsync();

        // Назначаем обслуживаемые адреса для каждой компании
        foreach (var company in companies)
        {
            // Каждая компания обслуживает от 1 до 5 адресов
            var addressesToServe = addresses
                .Where(a => a.Id != company.AddressId) // Исключаем юридический адрес
                .OrderBy(_ => random.Next())
                .Take(random.Next(1, 6))
                .ToList();

            foreach (var address in addressesToServe)
            {
                company.ServedAddresses.Add(address);
            }
        }

        await context.SaveChangesAsync();

        Log.Information($"Seeded {companies.Count} companies");
        return companies;
    }

    private static async Task<List<Request>> SeedRequestsAsync(HooomeDbContext context)
    {
        if (await context.Requests.AnyAsync())
            return await context.Requests.ToListAsync();

        // Получаем существующие адреса из базы
        var addresses = await context.Addresses.ToListAsync();
        if (!addresses.Any())
        {
            Log.Warning("No addresses found. Run SeedAddressesAsync first.");
            return new List<Request>();
        }

        var categories = new[]
        {
        RequestCategory.HotWaterSupply,
        RequestCategory.ColdWaterSupply,
        RequestCategory.PowerSupply,
        RequestCategory.Heating,
        RequestCategory.ElevatorMaintenance,
        RequestCategory.RoofingWorks,
        RequestCategory.Sewage,
        RequestCategory.ApartmentBuildingSanitation,
        RequestCategory.TerritorySanitation,
        RequestCategory.StreetLighting,
        RequestCategory.RoadsAndSidewalks
    };

        var statuses = new[]
        {
        RequestStatus.Created,
        RequestStatus.InProgress,
        RequestStatus.Completed,
        RequestStatus.Rejected
    };

        var requests = new List<Request>();
        var random = new Random();

        // Создаём 50-100 заявок для разнообразия
        var requestsCount = random.Next(50, 101);

        for (int i = 1; i <= requestsCount; i++)
        {
            // Выбираем случайный адрес из существующих
            var address = addresses[random.Next(addresses.Count)];
            var category = categories[random.Next(categories.Length)];
            var status = statuses[random.Next(statuses.Length)];
            var createdDate = DateTime.UtcNow.AddDays(-random.Next(1, 90));

            // Некоторые заявки могут быть с фото, некоторые без
            var hasPhoto = random.Next(3) == 0;

            requests.Add(new Request
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(), // В реальном приложении нужно брать ID существующего пользователя
                AddressId = address.Id,
                Title = "asdasd",
                Description = GetRequestDescription(category, address, i),
                Status = status,
                Category = category,
                CreatedAt = createdDate,
                UpdatedAt = status != RequestStatus.Created ? createdDate.AddDays(random.Next(1, 15)) : null
            });
        }

        // Добавляем несколько заявок на один адрес (для демонстрации кластеризации)
        var popularAddress = addresses.FirstOrDefault();
        if (popularAddress != null)
        {
            for (int i = 1; i <= 5; i++)
            {
                var category = categories[random.Next(categories.Length)];
                var status = statuses[random.Next(statuses.Length)];
                var createdDate = DateTime.UtcNow.AddDays(-random.Next(1, 30));

                requests.Add(new Request
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    AddressId = popularAddress.Id,
                    Title = "asdasd",
                    Description = $"Повторная заявка на адрес {popularAddress.Street}, {popularAddress.HouseNumber}. Проблема не решена с предыдущей заявки.",
                    Status = status,
                    Category = category,
                    CreatedAt = createdDate,
                    UpdatedAt = status != RequestStatus.Created ? createdDate.AddDays(random.Next(1, 7)) : null
                });
            }
        }

        await context.Requests.AddRangeAsync(requests);
        await context.SaveChangesAsync();

        Log.Information($"Added {requests.Count} requests for {addresses.Count} addresses");

        return requests;
    }

    private static string GetRequestDescription(RequestCategory category, Address address, int index)
    {
        var baseDescription = category switch
        {
            RequestCategory.HotWaterSupply => $"По адресу {address.Street}, {address.HouseNumber} отсутствует горячая вода уже {new Random().Next(2, 7)} дня. Прошу принять меры.",
            RequestCategory.ColdWaterSupply => $"Из крана течёт ржавая вода с примесями. Адрес: {address.Street}, {address.HouseNumber}.",
            RequestCategory.PowerSupply => $"В доме {address.Street}, {address.HouseNumber} периодически отключается свет. Электрики не могут найти причину.",
            RequestCategory.Heating => $"Батареи холодные при температуре на улице -{new Random().Next(5, 15)}°C. В квартире {index} температура опустилась до 16°C.",
            RequestCategory.ElevatorMaintenance => $"Лифт в подъезде {new Random().Next(1, 5)} не работает уже 3 дня. Жильцам приходится подниматься пешком.",
            RequestCategory.RoofingWorks => $"После дождя в квартире {index} на {new Random().Next(3, 9)} этаже потекла крыша. Требуется срочный ремонт.",
            RequestCategory.Sewage => $"Забилась канализация в стояке. Вода поднимается на {new Random().Next(2, 5)} этаж.",
            RequestCategory.ApartmentBuildingSanitation => $"Мусоропровод забит отходами, неприятный запах на всех этажах.",
            RequestCategory.TerritorySanitation => $"Двор не убирают уже месяц, мусорные баки переполнены.",
            RequestCategory.StreetLighting => $"Фонарь во дворе дома {address.Street}, {address.HouseNumber} не горит месяц. Темно и небезопасно.",
            RequestCategory.RoadsAndSidewalks => $"Тротуар возле дома разбит, люди спотыкаются. Необходим ремонт.",
            _ => $"Обращение жильца дома {address.Street}, {address.HouseNumber}. Требуется помощь ЖЭСа."
        };

        // Добавляем срочность для некоторых заявок
        var urgentSuffixes = new[] { " Срочно!", " Требуется немедленное вмешательство!", "" };
        var urgent = urgentSuffixes[new Random().Next(0, urgentSuffixes.Length)];

        return baseDescription + urgent;
    }

    private static async Task SeedComplaintsAsync(HooomeDbContext context, List<Company> companies, List<Request> requests)
    {
        if (await context.Complaints.AnyAsync()) return;

        var complaints = new List<Complaint>
    {
        new Complaint
        {
            Id = Guid.NewGuid(),
            ShortDescription = "Плохая работа УК",
            Description = "Управляющая компания не выполняет свои обязанности",
            Type = ComplaintType.Resident,
            Status = ComplaintStatus.OnReview,
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        },
        new Complaint
        {
            Id = Guid.NewGuid(),
            ShortDescription = "Заявка не выполнена",
            Description = "Подал заявку 2 недели назад, никто не пришёл",
            Type = ComplaintType.Request,
            Status = ComplaintStatus.AcceptedForReview,
            CreatedAt = DateTime.UtcNow.AddDays(-5)
        },
        new Complaint
        {
            Id = Guid.NewGuid(),
            ShortDescription = "Ошибка в приложении",
            Description = "При выборе даты в форме заявки приложение вылетает",
            Type = ComplaintType.Hooome,
            Status = ComplaintStatus.Closed,
            CreatedAt = DateTime.UtcNow.AddDays(-20),
            UpdatedAt = DateTime.UtcNow.AddDays(-15)
        }
    };

        await context.Complaints.AddRangeAsync(complaints);
        await context.SaveChangesAsync();

        Log.Information($"Seeded {complaints.Count} complaints");
    }

    private static async Task SeedPollsAsync(HooomeDbContext context)
    {
        if (await context.Polls.AnyAsync()) return;

        var random = new Random();
        var polls = new List<Poll>();

        // Получаем существующие компании
        var companies = await context.Companies.ToListAsync();
        if (!companies.Any())
        {
            Log.Warning("No companies found, skipping Polls seeding");
            return;
        }

        // Данные для голосований
        var pollTemplates = new[]
        {
        new {
            Title = "Выбор управляющей компании",
            Description = "Какая компания лучше справляется с обслуживанием?",
            Type = PollType.One,
            Options = new[] { "ЖилКомСервис", "Уютный Дом", "Городской Коммунальщик", "Комфорт Плюс" }
        },
        new {
            Title = "Оценка качества уборки",
            Description = "Как вы оцениваете качество уборки придомовой территории?",
            Type = PollType.Several,
            Options = new[] { "Отлично", "Хорошо", "Удовлетворительно", "Плохо" }
        },
        new {
            Title = "Приоритетные направления работ",
            Description = "Какие работы нужно выполнить в первую очередь?",
            Type = PollType.Several,
            Options = new[] { "Ремонт подъездов", "Благоустройство двора", "Ремонт кровли", "Замена лифтов", "Освещение" }
        },
        new {
            Title = "Удобство работы приложения",
            Description = "Оцените удобство использования нашего приложения",
            Type = PollType.One,
            Options = new[] { "Очень удобно", "Удобно", "Неудобно", "Не пользуюсь" }
        },
        new {
            Title = "График вывоза мусора",
            Description = "Устраивает ли вас текущий график вывоза ТБО?",
            Type = PollType.One,
            Options = new[] { "Да", "Нет, нужно чаще", "Нет, нужно реже", "Затрудняюсь ответить" }
        }
    };

        var createdBy = "11111111-1111-1111-1111-111111111111"; // Системный пользователь

        foreach (var template in pollTemplates)
        {
            var company = companies[random.Next(companies.Count)];

            var poll = new Poll
            {
                Id = Guid.NewGuid(),
                Title = template.Title,
                Description = template.Description,
                CreatedBy = Guid.Parse(createdBy),
                CompanyId = company.Id,
                Status = PollStatus.Active,
                Type = template.Type,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                Options = new List<PollOption>()
            };

            // Добавляем варианты ответов
            foreach (var optionContent in template.Options)
            {
                poll.Options.Add(new PollOption
                {
                    Id = Guid.NewGuid(),
                    PollId = poll.Id,
                    Content = optionContent,
                    CreatedAt = poll.CreatedAt
                });
            }

            polls.Add(poll);
        }

        await context.Polls.AddRangeAsync(polls);
        await context.SaveChangesAsync();

        // Добавляем голоса (опционально)
        await SeedPollVotesAsync(context, polls);

        Log.Information($"Seeded {polls.Count} polls");
    }

    private static async Task SeedPollVotesAsync(HooomeDbContext context, List<Poll> polls)
    {
        if (await context.PollVotes.AnyAsync()) return;

        var random = new Random();
        var allVotes = new List<PollVote>();

        // Получаем пользователей (можно добавить тестовых)
        var defaultUserId = "11111111-1111-1111-1111-111111111111";

        foreach (var poll in polls)
        {
            // Количество голосов от 0 до 50
            var votesCount = random.Next(0, 51);

            for (int i = 0; i < votesCount; i++)
            {
                var optionsList = poll.Options.ToList();
                var option = optionsList[random.Next(optionsList.Count)];

                allVotes.Add(new PollVote
                {
                    Id = Guid.NewGuid(),
                    PollId = poll.Id,
                    OptionId = option.Id,
                    UserId = Guid.Parse(defaultUserId),
                    CreatedAt = poll.CreatedAt.AddMinutes(random.Next(1, 1440))
                });
            }
        }

        if (allVotes.Any())
        {
            await context.PollVotes.AddRangeAsync(allVotes);
            await context.SaveChangesAsync();
            Log.Information($"Seeded {allVotes.Count} poll votes");
        }
    }

    private static async Task SeedPollOptionsAsync(HooomeDbContext context)
    {
        if (await context.PollOptions.AnyAsync()) return;

        var polls = await context.Polls.ToListAsync();
        var options = new List<PollOption>();
        var random = new Random();

        var optionSets = new Dictionary<string, string[]>
        {
            ["Качество уборки подъездов"] = new[] { "Отлично", "Хорошо", "Удовлетворительно", "Плохо", "Ужасно" },
            ["Благоустройство дворовой территории"] = new[] { "Детская площадка", "Спортивная площадка", "Парковка", "Зеленая зона", "Место для выгула собак" },
            ["Работа управляющей компании"] = new[] { "Отлично", "Хорошо", "Удовлетворительно", "Плохо", "Очень плохо" },
            ["Освещение во дворе"] = new[] { "Достаточно", "Недостаточно", "Слишком ярко", "Не обращал внимания" },
            ["Вывоз мусора"] = new[] { "Да, устраивает", "Нет, нужно чаще", "Нет, нужно реже", "Не устраивает качество" },
            ["Детские площадки"] = new[] { "Да, срочно нужна", "Нет, существующая нормальная", "Нужен ремонт существующей", "Нужна площадка для подростков" },
            ["Парковочные места"] = new[] { "Нужна подземная парковка", "Нужна многоуровневая парковка", "Расширить существующую", "Запретить въезд во двор" },
            ["Озеленение территории"] = new[] { "Лиственные деревья", "Хвойные деревья", "Кустарники", "Цветники", "Газоны" },
            ["Капитальный ремонт"] = new[] { "Кровля", "Фасад", "Подвал", "Коммуникации", "Лифт" },
            ["Общественный транспорт"] = new[] { "Устраивает", "Нужно больше маршрутов", "Нужно чаще", "Нужно заменить транспорт" }
        };

        foreach (var poll in polls)
        {
            var optionsArray = optionSets.ContainsKey(poll.Title)
                ? optionSets[poll.Title]
                : new[] { "Вариант 1", "Вариант 2", "Вариант 3", "Вариант 4", "Вариант 5" };

            foreach (var option in optionsArray)
            {
                options.Add(new PollOption
                {
                    Id = Guid.NewGuid(),
                    PollId = poll.Id,
                    Content = option,
                    CreatedAt = poll.CreatedAt.AddMinutes(random.Next(1, 60))
                });
            }
        }

        await context.PollOptions.AddRangeAsync(options);
        await context.SaveChangesAsync();
        Log.Information($"Added {options.Count} poll options");
    }

    private static async Task SeedPollVotesAsync(HooomeDbContext context)
    {
        if (await context.PollVotes.AnyAsync()) return;

        var polls = await context.Polls.ToListAsync();
        var options = await context.PollOptions.ToListAsync();
        var votes = new List<PollVote>();
        var random = new Random();

        foreach (var poll in polls)
        {
            var pollOptions = options.Where(o => o.PollId == poll.Id).ToList();
            if (!pollOptions.Any()) continue;

            // Генерируем от 5 до 15 голосов на опрос
            var votesCount = random.Next(5, 16);
            var userIds = new HashSet<Guid>();

            for (int i = 0; i < votesCount; i++)
            {
                var userId = Guid.NewGuid();
                if (userIds.Contains(userId)) continue;

                userIds.Add(userId);

                votes.Add(new PollVote
                {
                    Id = Guid.NewGuid(),
                    PollId = poll.Id,
                    OptionId = pollOptions[random.Next(pollOptions.Count)].Id,
                    UserId = userId,
                    CreatedAt = poll.CreatedAt.AddDays(random.Next(1, 10))
                });
            }
        }

        await context.PollVotes.AddRangeAsync(votes);
        await context.SaveChangesAsync();
        Log.Information($"Added {votes.Count} poll voites");
    }

    private static async Task SeedWorksAsync(HooomeDbContext context)
    {
        if (await context.Works.AnyAsync()) return;

        var random = new Random();
        var works = new List<Work>();

        var addresses = await context.Addresses.Take(10).ToListAsync();
        if (!addresses.Any()) return;

        var now = DateTime.UtcNow;

        for (int i = 0; i < 30; i++)
        {
            var address = addresses[random.Next(addresses.Count)];

            works.Add(new Work
            {
                Id = Guid.NewGuid(),
                Title = $"Работа №{i + 1}",
                Description = $"Описание работ по адресу {address.Street}, {address.HouseNumber}",
                AddressId = address.Id,
                Category = (RequestCategory)random.Next(3, 12),
                Seriousness = (WorkSeriousness)random.Next(1, 3),
                PlannedStartTime = now.AddDays(random.Next(1, 30)),
                PlannedEndTime = now.AddDays(random.Next(31, 60)),
                CreatedAt = now
            });
        }

        await context.Works.AddRangeAsync(works);
        await context.SaveChangesAsync();
    }
}
