using EFCore.BulkExtensions;
using Hooome.Domain;
using Hooome.Domain.Enums;
using Hooome.Persistance;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Globalization;
using System.Text;

namespace Hooome.WebApi.Services;

public class DataSeeder
{
    private static DateTime Utc(DateTime dt) =>
        dt.Kind == DateTimeKind.Utc ? dt : DateTime.SpecifyKind(dt, DateTimeKind.Utc);

    private static DateTime? UtcOrNull(DateTime? dt) =>
        dt.HasValue ? Utc(dt.Value) : null;

    private static readonly string[] ByPhonePrefixes = { "+375 (29)", "+375 (33)", "+375 (44)", "+375 (25)" };
    private static readonly string[] ByEmailDomains = { "@tut.by", "@mail.by", "@gmail.com", "@bk.by" };

    private static readonly string[] MinskDistricts = {
        "Центральный", "Советский", "Октябрьский", "Партизанский",
        "Заводской", "Ленинский", "Московский", "Фрунзенский", "Первомайский"
    };

    private static readonly string[] MinskStreets = {
        "проспект Независимости", "проспект Победителей", "проспект Машерова",
        "улица Богдановича", "улица Якуба Коласа", "улица Сурганова", "улица Мельникайте",
        "улица Калиновского", "улица Притыцкого", "улица Тимирязева", "улица Кирова",
        "улица Советская", "улица Октябрьская", "улица Немига", "улица Романовская Слобода",
        "улица Шаранговича", "улица Чкалова", "улица Волгоградская", "улица Ландера",
        "улица Бумажкова", "улица Казинца", "улица Пулихова", "улица Маяковского"
    };

    private static readonly string[] MinskUtilityCompanies = {
        "ЖРЭО №1 Центрального района", "ЖРЭО №2 Советского района", "ЖРЭО №3 Октябрьского района",
        "ЖРЭО №1 Партизанского района", "ЖРЭО №1 Заводского района", "ЖРЭО №1 Ленинского района",
        "ЖРЭО №1 Московского района", "ЖРЭО №1 Фрунзенского района", "ЖРЭО №1 Первомайского района",
        "ЖЭС №44", "ЖЭС №77", "ЖЭС №120", "ЖЭС №156", "ЖЭС №189",
        "ТСЖ «Зелёный двор»", "ТСЖ «Уют»", "ТСЖ «Наш дом»", "ТСЖ «Комфорт»",
        "ОАО «Минское городское жилищное объединение»"
    };

    public async Task SeedAllDataAsync(HooomeDbContext context)
    {
        try
        {
            await SeedAddressesAsync(context);
            var companies = await SeedCompaniesAsync(context);
            var requests = await SeedRequestsAsync(context);
            await SeedComplaintsAsync(context, companies, requests);
            var polls = await SeedPollsAsync(context);
            await SeedPollVotesAsync(context, polls);
            await SeedWorksAsync(context);

            await context.SaveChangesAsync();

            Log.Information("✅ All data has been added successfully (Minsk, UTC dates)");
        }
        catch (Exception ex)
        {
            Log.Error($"❌ Error adding data: {ex.Message}");
            throw;
        }
    }

    private static async Task SeedAddressesAsync(HooomeDbContext context)
    {
        if (await context.Addresses.AnyAsync())
            return;

        var lines = await File.ReadAllLinesAsync("static/minsk_addresses.csv", Encoding.UTF8);
        var addresses = new List<Address>();
        var culture = System.Globalization.CultureInfo.InvariantCulture;

        Console.WriteLine($"📍 Start loading Minsk addresses");

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split(';');
            if (parts.Length >= 5)
            {
                try
                {
                    var street = parts[2].Trim().Trim('"');
                    var houseNumber = parts[1].Trim().Trim('"');

                    decimal? latitude = null;
                    decimal? longitude = null;

                    if (parts.Length > 3 && !string.IsNullOrWhiteSpace(parts[3]))
                    {
                        var latString = parts[3].Trim().Trim('"');
                        if (decimal.TryParse(latString, NumberStyles.Any, culture, out decimal lat))
                            latitude = lat;
                    }

                    if (parts.Length > 4 && !string.IsNullOrWhiteSpace(parts[4]))
                    {
                        var lonString = parts[4].Trim().Trim('"');
                        if (decimal.TryParse(lonString, NumberStyles.Any, culture, out decimal lon))
                            longitude = lon;
                    }

                    street = street.Replace('%', ' ').Replace("  ", " ").Trim();
                    if (houseNumber.Length > 50) houseNumber = houseNumber[..50];

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
                    Console.WriteLine($"⚠️ Error processing line: {ex.Message}");
                }
            }

            if (addresses.Count % 1000 == 0 && addresses.Count > 0)
                Console.WriteLine($"Processed {addresses.Count} addresses...");
        }

        if (addresses.Any())
        {
            await context.BulkInsertAsync(addresses);
            Console.WriteLine($"✅ Saved {addresses.Count} Minsk addresses");
            Log.Information($"Seeded {addresses.Count} addresses");
        }
    }

    private static string GenerateMinskHouseNumber(Random random)
    {
        var formats = new[]
        {
            $"{random.Next(1, 200)}",
            $"{random.Next(1, 200)}к{random.Next(1, 5)}",
            $"{random.Next(1, 200)}с{random.Next(1, 3)}",
            $"{random.Next(1, 200)}/{random.Next(1, 10)}",
            $"{random.Next(1, 200)}А",
            $"{random.Next(1, 200)}Б",
        };
        return formats[random.Next(formats.Length)];
    }

    private static (decimal, decimal) GenerateMinskCoordinates(Random random)
    {
        var baseLat = 53.9045m;
        var baseLon = 27.5615m;
        var lat = baseLat + (decimal)(random.NextDouble() - 0.5) * 0.22m;
        var lon = baseLon + (decimal)(random.NextDouble() - 0.5) * 0.32m;
        return (Math.Round(lat, 6), Math.Round(lon, 6));
    }

    private static async Task<List<Company>> SeedCompaniesAsync(HooomeDbContext context)
    {
        if (await context.Companies.AnyAsync())
            return await context.Companies.ToListAsync();

        var companies = new List<Company>();
        var random = new Random(42);
        var addresses = await context.Addresses.ToListAsync();

        if (!addresses.Any())
        {
            Log.Warning("⚠️ No addresses found for companies");
            return companies;
        }

        foreach (var companyName in MinskUtilityCompanies)
        {
            var legalAddress = addresses[random.Next(addresses.Count)];

            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = companyName,
                Phone = GenerateBelarusPhone(random),
                Email = GenerateBelarusEmail(companyName, random),
                WorkingHours = GenerateWorkingHours(random),
                CreatedAt = Utc(DateTime.UtcNow.AddDays(-random.Next(365, 1095))),
                UpdatedAt = null,
                AddressId = legalAddress.Id,
                Address = legalAddress,
                ServedAddresses = new List<Address>(),
                Polls = new List<Poll>(),
                Comments = new List<RequestComment>()
            };

            companies.Add(company);
        }

        await context.Companies.AddRangeAsync(companies);
        await context.SaveChangesAsync();

        foreach (var company in companies)
        {
            var served = addresses
                .Where(a => a.Id != company.AddressId)
                .OrderBy(_ => random.Next())
                .Take(random.Next(8, 25))
                .ToList();

            foreach (var addr in served)
            {
                addr.ServicedByCompanyId = company.Id;
                company.ServedAddresses.Add(addr);
            }
        }

        await context.SaveChangesAsync();
        Log.Information($"✅ Seeded {companies.Count} Minsk companies");
        return companies;
    }

    private static string GenerateBelarusPhone(Random random)
    {
        var prefix = ByPhonePrefixes[random.Next(ByPhonePrefixes.Length)];
        return $"{prefix} {random.Next(100, 999)}-{random.Next(10, 99)}-{random.Next(10, 99):D2}";
    }

    private static string GenerateBelarusEmail(string companyName, Random random)
    {
        var name = companyName.ToLower()
            .Replace("жрэо", "zhreo").Replace("жэс", "zhes")
            .Replace("тсж", "tszh").Replace("оао", "").Replace("уп", "")
            .Replace(" ", "").Replace("№", "").Replace("района", "").Trim('.');
        return $"{name}{random.Next(1, 99)}{ByEmailDomains[random.Next(ByEmailDomains.Length)]}";
    }

    private static string GenerateWorkingHours(Random random)
    {
        var schedules = new[]
        {
            "Пн-Пт 8:00-17:00, перерыв 13:00-13:45",
            "Пн-Пт 9:00-18:00",
            "Пн-Чт 8:00-17:00, Пт 8:00-15:45",
            "Пн-Пт 8:30-17:30, сб 9:00-13:00 (аварийная)",
            "Круглосуточно (аварийная служба)"
        };
        return schedules[random.Next(schedules.Length)];
    }

    private static async Task<List<Request>> SeedRequestsAsync(HooomeDbContext context)
    {
        if (await context.Requests.AnyAsync())
            return await context.Requests.ToListAsync();

        var addresses = await context.Addresses.ToListAsync();
        if (!addresses.Any()) return new List<Request>();

        var categories = Enum.GetValues<RequestCategory>();
        var statuses = Enum.GetValues<RequestStatus>();
        var requests = new List<Request>();
        var random = new Random(42);
        var requestsCount = random.Next(200, 350);

        for (int i = 1; i <= requestsCount; i++)
        {
            var address = addresses[random.Next(addresses.Count)];
            var category = categories[random.Next(categories.Length)];
            var status = statuses[random.Next(statuses.Length)];
            var createdDate = Utc(DateTime.UtcNow.AddDays(-random.Next(1, 120)));

            requests.Add(new Request
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                AddressId = address.Id,
                Address = address,
                Title = GenerateRequestTitle(category),
                Description = GetMinskRequestDescription(category, address, random),
                Status = status,
                Category = category,
                CreatedAt = createdDate,
                UpdatedAt = status != RequestStatus.Created
                    ? UtcOrNull(createdDate.AddDays(random.Next(1, 20)))
                    : null,
                IsDeleted = false,
                Images = new List<RequestImage>(),
                Comments = new List<RequestComment>(),
                Notifications = new List<RequestNotification>()
            });
        }

        var hotAddresses = addresses.OrderBy(_ => random.Next()).Take(15).ToList();
        foreach (var addr in hotAddresses)
        {
            for (int i = 0; i < random.Next(2, 5); i++)
            {
                var category = categories[random.Next(categories.Length)];
                var createdDate = Utc(DateTime.UtcNow.AddDays(-random.Next(1, 14)));

                requests.Add(new Request
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    AddressId = addr.Id,
                    Address = addr,
                    Title = GenerateRequestTitle(category),
                    Description = $"Повторное обращение: {GetMinskRequestDescription(category, addr, random)}",
                    Status = RequestStatus.InProgress,
                    Category = category,
                    CreatedAt = createdDate,
                    UpdatedAt = UtcOrNull(DateTime.UtcNow.AddDays(-random.Next(0, 3))),
                    IsDeleted = false,
                    Images = new List<RequestImage>(),
                    Comments = new List<RequestComment>(),
                    Notifications = new List<RequestNotification>()
                });
            }
        }

        await context.Requests.AddRangeAsync(requests);
        await context.SaveChangesAsync();
        Log.Information($"✅ Seeded {requests.Count} Minsk requests");
        return requests;
    }

    private static string GenerateRequestTitle(RequestCategory category) => category switch
    {
        RequestCategory.HotWaterSupply => "Отсутствует горячая вода",
        RequestCategory.ColdWaterSupply => "Проблемы с холодным водоснабжением",
        RequestCategory.PowerSupply => "Перебои с электроснабжением",
        RequestCategory.Heating => "Нет отопления / холодные батареи",
        RequestCategory.ElevatorMaintenance => "Не работает лифт",
        RequestCategory.RoofingWorks => "Протечка крыши",
        RequestCategory.Sewage => "Засор канализации",
        RequestCategory.ApartmentBuildingSanitation => "Не убирают подъезд",
        RequestCategory.TerritorySanitation => "Мусор во дворе не вывозят",
        RequestCategory.StreetLighting => "Не горит уличный фонарь",
        RequestCategory.RoadsAndSidewalks => "Разбит тротуар / ямы во дворе",
        _ => "Обращение по вопросам ЖКХ"
    };

    private static string GetMinskRequestDescription(RequestCategory category, Address address, Random random)
    {
        var baseDesc = category switch
        {
            RequestCategory.HotWaterSupply =>
                $"г. Минск, {address.Street}, {address.HouseNumber}. Горячей воды нет уже {random.Next(2, 10)} дней. Диспетчер ЖРЭО сказал ждать. Когда будет вода?",
            RequestCategory.ColdWaterSupply =>
                $"Из крана течёт вода с ржавчиной. Адрес: {address.Street}, {address.HouseNumber}, Минск. Прошу проверить трубы.",
            RequestCategory.PowerSupply =>
                $"В доме {address.Street}, {address.HouseNumber} регулярно отключается свет. Звонили в Минскэнерго — проблем на линии нет.",
            RequestCategory.Heating =>
                $"Отопление включили, а батареи еле тёплые. {address.Street}, {address.HouseNumber}. Температура +16°C. Прошу наладить.",
            RequestCategory.ElevatorMaintenance =>
                $"Лифт в подъезде №{random.Next(1, 6)} дома {address.Street}, {address.HouseNumber} не работает {random.Next(2, 14)} дней.",
            RequestCategory.RoofingWorks =>
                $"После дождя на потолке появились пятна. {address.Street}, {address.HouseNumber}, Минск. Крыша требует ремонта.",
            RequestCategory.Sewage =>
                $"В ванной поднимается вода из канализации. {address.Street}, {address.HouseNumber}. Вызывайте аварийку!",
            RequestCategory.ApartmentBuildingSanitation =>
                $"Подъезд не убирают {random.Next(1, 4)} недели. {address.Street}, {address.HouseNumber}.",
            RequestCategory.TerritorySanitation =>
                $"Контейнерная площадка переполнена. {address.Street}, {address.HouseNumber}, Минск.",
            RequestCategory.StreetLighting =>
                $"Фонарь возле {address.Street}, {address.HouseNumber} не горит. Вечером темно и опасно.",
            RequestCategory.RoadsAndSidewalks =>
                $"Тротуар разбит, после дождя лужи. {address.Street}, {address.HouseNumber}.",
            _ => $"Обращение жителя Минска, {address.Street}, {address.HouseNumber}."
        };

        var endings = new[] { " Прошу принять меры.", " Заранее спасибо!", " С уважением, жилец.", "" };
        return baseDesc + endings[random.Next(endings.Length)];
    }

    private static async Task SeedComplaintsAsync(HooomeDbContext context, List<Company> companies, List<Request> requests)
    {
        if (await context.Complaints.AnyAsync()) return;

        var complaints = new List<Complaint>
        {
            new Complaint {
                Id = Guid.NewGuid(),
                ShortDescription = "ЖРЭО не реагирует на заявки",
                Description = "Подавал заявку на ремонт лифта 3 недели назад. Диспетчер говорит «в плане работ». Когда ремонт?",
                Type = ComplaintType.Resident,
                Status = ComplaintStatus.OnReview,
                CreatedAt = Utc(DateTime.UtcNow.AddDays(-21))
            },
            new Complaint {
                Id = Guid.NewGuid(),
                ShortDescription = "Некачественная уборка",
                Description = "Уборщица появляется раз в месяц. Подъезд грязный. пр. Независимости, 45, Минск.",
                Type = ComplaintType.Request,
                Status = ComplaintStatus.AcceptedForReview,
                CreatedAt = Utc(DateTime.UtcNow.AddDays(-7))
            },
            new Complaint {
                Id = Guid.NewGuid(),
                ShortDescription = "Баг в приложении",
                Description = "При отправке заявки с фото ошибка «Не удалось загрузить». Минск, Android 13, версия 2.4.1.",
                Type = ComplaintType.Hooome,
                Status = ComplaintStatus.AcceptedForReview,
                CreatedAt = Utc(DateTime.UtcNow.AddDays(-3))
            },
            new Complaint {
                Id = Guid.NewGuid(),
                ShortDescription = "Ошибка в квитанции",
                Description = "В ЕРИП завышены показания счётчика воды. ЖРЭО отказывается делать перерасчёт.",
                Type = ComplaintType.Resident,
                Status = ComplaintStatus.OnReview,
                CreatedAt = Utc(DateTime.UtcNow.AddDays(-14))
            }
        };

        await context.Complaints.AddRangeAsync(complaints);
        await context.SaveChangesAsync();
        Log.Information($"✅ Seeded {complaints.Count} complaints");
    }

    private static async Task<List<Poll>> SeedPollsAsync(HooomeDbContext context)
    {
        if (await context.Polls.AnyAsync()) return [];

        var random = new Random(42);
        var polls = new List<Poll>();
        var companies = await context.Companies.ToListAsync();
        if (!companies.Any()) return [];

        var pollTemplates = new[]
        {
            new { Title = "Оценка работы ЖРЭО", Description = "Как вы оцениваете качество обслуживания?", Type = PollType.One, Options = new[] { "Отлично", "Хорошо", "Удовлетворительно", "Плохо", "Очень плохо" } },
            new { Title = "Благоустройство двора", Description = "Что улучшить в первую очередь?", Type = PollType.Several, Options = new[] { "Детская площадка", "Парковка", "Освещение", "Озеленение", "Ремонт тротуаров" } },
            new { Title = "Удобство приложения", Description = "Насколько удобно подавать заявки?", Type = PollType.One, Options = new[] { "Очень удобно", "Удобно", "Неудобно", "Не пользуюсь" } },
            new { Title = "Вывоз мусора", Description = "Устраивает ли график вывоза ТКО?", Type = PollType.One, Options = new[] { "Да", "Нет, чаще", "Нет, реже", "Не замечаю" } }
        };

        var systemUser = Guid.Parse("11111111-1111-1111-1111-111111111111");

        foreach (var tpl in pollTemplates)
        {
            var poll = new Poll
            {
                Id = Guid.NewGuid(),
                Title = tpl.Title,
                Description = tpl.Description,
                CreatedBy = systemUser,
                CompanyId = companies[random.Next(companies.Count)].Id,
                Status = PollStatus.Active,
                Type = tpl.Type,
                CreatedAt = Utc(DateTime.UtcNow.AddDays(-random.Next(1, 45))),
                Options = new List<PollOption>(),
                Votes = new List<PollVote>()
            };

            foreach (var opt in tpl.Options)
            {
                poll.Options.Add(new PollOption
                {
                    Id = Guid.NewGuid(),
                    PollId = poll.Id,
                    Content = opt,
                    CreatedAt = Utc(DateTime.UtcNow),
                    Votes = new List<PollVote>()
                });
            }
            polls.Add(poll);
        }

        await context.Polls.AddRangeAsync(polls);
        await context.SaveChangesAsync();
        Log.Information($"✅ Seeded {polls.Count} polls");
        return polls;
    }

    private static async Task SeedPollVotesAsync(HooomeDbContext context, List<Poll> polls)
    {
        if (await context.PollVotes.AnyAsync()) return;

        var random = new Random(42);
        var votes = new List<PollVote>();

        foreach (var poll in polls)
        {
            var options = poll.Options.ToList();
            if (!options.Any()) continue;

            foreach (var _ in Enumerable.Range(0, random.Next(15, 60)))
            {
                votes.Add(new PollVote
                {
                    Id = Guid.NewGuid(),
                    PollId = poll.Id,
                    OptionId = options[random.Next(options.Count)].Id,
                    UserId = Guid.NewGuid(),
                    CreatedAt = Utc(poll.CreatedAt.AddHours(random.Next(1, 720)))
                });
            }
        }

        if (votes.Any())
        {
            await context.PollVotes.AddRangeAsync(votes);
            await context.SaveChangesAsync();
            Log.Information($"✅ Seeded {votes.Count} poll votes");
        }
    }

    private static async Task SeedWorksAsync(HooomeDbContext context)
    {
        if (await context.Works.AnyAsync()) return;

        var random = new Random(42);
        var works = new List<Work>();
        var addresses = await context.Addresses.ToListAsync();
        if (!addresses.Any()) return;

        var categories = Enum.GetValues<RequestCategory>().Skip(3).Take(9).ToArray();
        var now = Utc(DateTime.UtcNow);

        for (int i = 0; i < 50; i++)
        {
            var addr = addresses[random.Next(addresses.Count)];

            works.Add(new Work
            {
                Id = Guid.NewGuid(),
                Title = $"Работа №{i + 1}: {GenerateWorkTitle(random)}",
                Description = $"Плановые работы по адресу: г. Минск, {addr.Street}, {addr.HouseNumber}",
                AddressId = addr.Id,
                Address = addr,
                Category = categories[random.Next(categories.Length)],
                Seriousness = (WorkSeriousness)random.Next(1, 4),
                PlannedStartTime = Utc(now.AddDays(random.Next(1, 30))),
                PlannedEndTime = Utc(now.AddDays(random.Next(31, 60))),
                FactStartTime = null,
                FactEndTime = null,
                CreatedAt = now,
                UpdatedAt = null,
                Notifications = new List<WorkNotification>()
            });
        }

        await context.Works.AddRangeAsync(works);
        await context.SaveChangesAsync();
        Log.Information($"✅ Seeded {works.Count} planned works");
    }

    private static string GenerateWorkTitle(Random random)
    {
        var titles = new[] { "Ремонт кровли", "Замена стояков", "Благоустройство двора", "Ремонт подъезда", "Замена лифта", "Утепление фасада", "Ремонт электросетей", "Очистка ливнёвки" };
        return titles[random.Next(titles.Length)];
    }
}