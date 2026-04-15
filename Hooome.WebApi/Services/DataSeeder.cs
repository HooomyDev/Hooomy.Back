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
            //var companies = await SeedCompaniesAsync(context);
            //await SeedComplaintsAsync(context, companies, requests);
            await SeedAddressesAsync(context);
            var requests = await SeedRequestsAsync(context);
            //await SeedFavoriteAddressesAsync(context);
            //await SeedPollsAsync(context);
            //await SeedPollOptionsAsync(context);
            //await SeedPollVotesAsync(context);
            //await SeedRequestCommentsAsync(context, requests);
            //await SeedWorksAsync(context);

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

        var lines = await File.ReadAllLinesAsync("static/minsk_addresses.csv", Encoding.UTF8);
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
        return [];
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
                UserID = Guid.NewGuid(), // В реальном приложении нужно брать ID существующего пользователя
                AddressId = address.Id,
                Title = "asdasd",
                Description = GetRequestDescription(category, address, i),
                Status = status,
                Category = category,
                PhotoUrl = hasPhoto ? $"https://example.com/photos/request_{DateTime.UtcNow:yyyyMMdd}_{i}.jpg" : string.Empty,
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
                    UserID = Guid.NewGuid(),
                    AddressId = popularAddress.Id,
                    Title = "asdasd",
                    Description = $"Повторная заявка на адрес {popularAddress.Street}, {popularAddress.HouseNumber}. Проблема не решена с предыдущей заявки.",
                    Status = status,
                    Category = category,
                    PhotoUrl = random.Next(2) == 0 ? $"https://example.com/photos/repeat_{DateTime.UtcNow:yyyyMMdd}_{i}.jpg" : string.Empty,
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

        var types = new[] { ComplaintType.Request, ComplaintType.Management, ComplaintType.Hooome };
        var statuses = new[] { ComplaintStatus.AcceptedForReview, ComplaintStatus.OnReview, ComplaintStatus.Closed };
        var random = new Random();

        var complaints = new List<Complaint>();

        for (int i = 1; i <= 12; i++)
        {
            var type = types[random.Next(types.Length)];
            var hasRequest = type == ComplaintType.Request && random.Next(2) == 0;
            var hasCompany = type == ComplaintType.Management && random.Next(2) == 0;

            complaints.Add(new Complaint
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ShortDescription = GetComplaintShortDescription(i),
                Description = GetComplaintDescription(i),
                Type = type,
                Status = statuses[random.Next(statuses.Length)],
                RequestId = hasRequest ? requests[random.Next(requests.Count)].Id : null,
                CompanyId = hasCompany ? companies[random.Next(companies.Count)].Id : null,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                UpdatedAt = random.Next(2) == 0 ? DateTime.UtcNow.AddDays(-random.Next(1, 15)) : null
            });
        }

        await context.Complaints.AddRangeAsync(complaints);
        await context.SaveChangesAsync();
        Log.Information($"Added {complaints.Count} complaints");
    }

    private static async Task SeedFavoriteAddressesAsync(HooomeDbContext context)
    {
        if (await context.FavoriteAddresses.AnyAsync()) return;

        var streets = new[]
        {
            "ул. Немига",
            "пр-т Победителей",
            "ул. Кальварийская",
            "ул. Притыцкого",
            "ул. Гурского",
            "ул. Матусевича",
            "ул. Рафиева",
            "ул. Лобанка",
            "ул. Шаранговича",
            "ул. Алибегова",
            "ул. Кунцевщина",
            "ул. Горецкого"
        };

        var pseudonyms = new[]
        {
            "Дом", "Работа", "Дача", "Родители", "Школа", "Спортзал",
            "Магазин", "Парковка", "Гараж", "Участок", "Офис", "Квартира"
        };

        var addresses = new List<FavoriteAddress>();
        var random = new Random();

        for (int i = 1; i <= 15; i++)
        {
            var userId = Guid.NewGuid();

            // Каждому пользователю по 2-3 адреса
            for (int j = 0; j < random.Next(2, 4); j++)
            {
                addresses.Add(new FavoriteAddress
                {
                    Id = Guid.NewGuid(),
                    UserID = userId,
                    Pseudonym = pseudonyms[random.Next(pseudonyms.Length)] + (j > 0 ? $" {j + 1}" : ""),
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 60)),
                    UpdatedAt = random.Next(2) == 0 ? DateTime.UtcNow.AddDays(-random.Next(1, 30)) : null
                });
            }
        }

        await context.FavoriteAddresses.AddRangeAsync(addresses);
        await context.SaveChangesAsync();
        Log.Information($"Added {addresses.Count} favorite addresses");
    }

    private static async Task SeedPollsAsync(HooomeDbContext context)
    {
        if (await context.Polls.AnyAsync()) return;

        var company = await context.Companies.OrderByDescending(x => x.Name).FirstAsync();

        var polls = new List<Poll>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Качество уборки подъездов",
                Description = "Как вы оцениваете качество уборки в вашем подъезде?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-20),
                CompanyId = company.Id,
                Type = PollType.One
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Благоустройство дворовой территории",
                Description = "Что нужно улучшить в вашем дворе в первую очередь?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-18),
                CompanyId = company.Id,
                Type = PollType.One
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Работа управляющей компании",
                Description = "Оцените работу вашей управляющей компании за последний месяц",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                CompanyId = company.Id,
                Type = PollType.One
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Освещение во дворе",
                Description = "Достаточно ли освещен ваш двор в темное время суток?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-12),
                CompanyId = company.Id,
                Type = PollType.One
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Вывоз мусора",
                Description = "Устраивает ли вас график вывоза мусора?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                CompanyId = company.Id,
                Type = PollType.One
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Детские площадки",
                Description = "Нужна ли новая детская площадка в вашем дворе?",
                CreatedBy = Guid.NewGuid(),
                IsActive = false,
                CreatedAt = DateTime.UtcNow.AddDays(-25),
                CompanyId = company.Id,
                Type = PollType.One
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Парковочные места",
                Description = "Как решить проблему с парковкой во дворе?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-8),
                CompanyId = company.Id,
                Type = PollType.Several
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Озеленение территории",
                Description = "Какие деревья и кустарники вы хотели бы видеть во дворе?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                CompanyId = company.Id,
                Type = PollType.One
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Капитальный ремонт",
                Description = "Что нуждается в капитальном ремонте в первую очередь?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                CompanyId = company.Id,
                Type = PollType.One
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Общественный транспорт",
                Description = "Устраивает ли вас работа общественного транспорта в районе?",
                CreatedBy = Guid.NewGuid(),
                IsActive = false,
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                CompanyId = company.Id,
                Type = PollType.One
            }
        };

        await context.Polls.AddRangeAsync(polls);
        await context.SaveChangesAsync();
        Log.Information($"Added {polls.Count} polls");
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

        foreach (var poll in polls.Where(p => p.IsActive))
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

    private static async Task SeedRequestCommentsAsync(HooomeDbContext context, List<Request> requests)
    {
        if (await context.RequestComments.AnyAsync()) return;

        var comments = new List<RequestComment>();
        var random = new Random();

        var commentTexts = new[]
        {
            "Спасибо за быстрый ответ!",
            "Когда планируется выполнение работ?",
            "Проблема все еще актуальна",
            "Работы выполнены качественно, спасибо",
            "Пришлите, пожалуйста, дополнительную информацию",
            "Мастер уже приезжал, все исправил",
            "Когда придет специалист?",
            "Спасибо за оперативность!",
            "Повторно обращаюсь с этой проблемой",
            "Все отлично, спасибо за работу",
            "Можно ли ускорить выполнение?",
            "Проблема решена, спасибо"
        };

        foreach (var request in requests)
        {
            // От 1 до 5 комментариев на заявку
            var commentsCount = random.Next(1, 6);

            for (int i = 0; i < commentsCount; i++)
            {
                comments.Add(new RequestComment
                {
                    Id = Guid.NewGuid(),
                    RequestId = request.Id,
                    UserId = Guid.NewGuid(),
                    Text = commentTexts[random.Next(commentTexts.Length)],
                    PhotoUrl = random.Next(4) == 0 ? $"https://example.com/photos/comment{i}.jpg" : string.Empty,
                    CreatedAt = request.CreatedAt.AddDays(random.Next(1, 15)),
                    UpdatedAt = random.Next(3) == 0 ? request.CreatedAt.AddDays(random.Next(2, 20)) : null
                });
            }
        }

        await context.RequestComments.AddRangeAsync(comments);
        await context.SaveChangesAsync();
        Log.Information($"Added {comments.Count} comments");
    }

    private static async Task SeedWorksAsync(HooomeDbContext context)
    {
        if (await context.Works.AnyAsync()) return;

        var random = new Random();
        var works = new List<Work>();

        var streets = new[]
        {
            "ул. Немига", "пр-т Победителей", "ул. Кальварийская", "ул. Притыцкого",
            "ул. Гурского", "ул. Матусевича", "ул. Рафиева", "ул. Лобанка",
            "ул. Шаранговича", "ул. Алибегова", "ул. Кунцевщина", "ул. Горецкого"
        };

        var workTitles = new[]
        {
            "Ремонт теплотрассы", "Замена лифта", "Ямочный ремонт дороги",
            "Обрезка деревьев", "Установка детской площадки", "Ремонт кровли",
            "Промывка системы отопления", "Замена светильников", "Уборка территории",
            "Покраска фасада", "Ремонт подъезда", "Замена труб водоснабжения"
        };

        var categories = new[]
        {
            RequestCategory.Heating,
            RequestCategory.ElevatorMaintenance,
            RequestCategory.RoadsAndSidewalks,
            RequestCategory.TerritoryImprovement,
            RequestCategory.PublicPlacesParksSquares,
            RequestCategory.RoofingWorks,
            RequestCategory.GeneralConstruction,
            RequestCategory.StreetLighting,
            RequestCategory.TerritorySanitation,
            RequestCategory.ApartmentBuildingSanitation
        };

        for (int i = 1; i <= 15; i++)
        {
            var plannedStart = DateTime.UtcNow.AddDays(random.Next(1, 30));
            var seriousness = random.Next(3) switch
            {
                0 => WorkSeriousness.Info,
                1 => WorkSeriousness.Warn,
                _ => WorkSeriousness.Unknown
            };

            works.Add(new Work
            {
                Id = Guid.NewGuid(),
                Title = workTitles[random.Next(workTitles.Length)] + $" {i}",
                Description = $"Плановые работы по адресу. Необходимо выполнить работы согласно графику.",
                Category = categories[random.Next(categories.Length)],
                Seriousness = seriousness,
                PlannedStartTime = plannedStart,
                PlannedEndTime = plannedStart.AddDays(random.Next(1, 14)),
                FactStartTime = random.Next(2) == 0 ? plannedStart.AddHours(-random.Next(1, 5)) : (DateTime?)null,
                FactEndTime = random.Next(2) == 0 ? plannedStart.AddDays(random.Next(1, 7)) : (DateTime?)null,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 20)),
                UpdatedAt = random.Next(2) == 0 ? DateTime.UtcNow.AddDays(-random.Next(1, 10)) : null
            });
        }

        await context.Works.AddRangeAsync(works);
        await context.SaveChangesAsync();
        Log.Information($"Added {works.Count} works");
    }

    // Вспомогательные методы

    private static string GetRequestDescription(RequestCategory category, int index)
    {
        return category switch
        {
            RequestCategory.HotWaterSupply => $"Уже третью неделю нет горячей воды в квартире {index}. Обращались в ЖЭС, но проблема не решается.",
            RequestCategory.ColdWaterSupply => $"Из крана течет ржавая вода. Просим провести промывку системы и замену фильтров.",
            RequestCategory.PowerSupply => $"В подъезде регулярно отключается свет. Особенно в вечернее время. Требуется проверка проводки.",
            RequestCategory.Heating => $"Батареи еле теплые, в квартире холодно. Просим проверить систему отопления и увеличить подачу тепла.",
            RequestCategory.ElevatorMaintenance => $"Лифт застрял между этажами. В кабине никого нет, но требуется срочный ремонт.",
            RequestCategory.RoofingWorks => $"После дождя вода течет по стенам в подъезде. Требуется срочный ремонт кровли.",
            RequestCategory.Sewage => $"Неприятный запах из подвала. Вероятно, засор канализации.",
            RequestCategory.ApartmentBuildingSanitation => $"Соседи оставляют мусор на лестничной клетке. Просим провести беседу и усилить уборку.",
            RequestCategory.TerritorySanitation => $"Мусорные баки переполнены, отходы разлетаются по двору. Увеличьте частоту вывоза.",
            RequestCategory.StreetLighting => $"Фонарь возле подъезда не работает уже месяц. В темноте небезопасно ходить.",
            RequestCategory.RoadsAndSidewalks => $"Глубокая яма на проезжей части возле дома. Повреждаются автомобили.",
            _ => $"Подробное описание заявки №{index}. Требуется вмешательство специалистов."
        };
    }

    private static string GetComplaintShortDescription(int index)
    {
        var complaints = new[]
        {
            "Некачественное обслуживание",
            "Грубость сотрудника",
            "Несвоевременный вывоз мусора",
            "Плохая уборка подъезда",
            "Отсутствие отопления",
            "Проблемы с водоснабжением",
            "Шумные соседи",
            "Незаконная парковка",
            "Повреждение имущества",
            "Неисправность лифта",
            "Залив квартиры",
            "Бездействие ЖЭСа"
        };

        return complaints[index % complaints.Length];
    }

    private static string GetComplaintDescription(int index)
    {
        return $"Подробное описание жалобы №{index}. Проблема требует вмешательства и решения в кратчайшие сроки. " +
               $"Неоднократно обращались в различные инстанции, но реакции нет.";
    }
}