using Hooome.Domain;
using Hooome.Domain.Enums;
using Hooome.Persistance;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Hooome.WebApi.Services;

public class DataSeeder
{
    public async Task SeedAllDataAsync(HooomeDbContext context)
    {
        try
        {
            var companies = await SeedCompaniesAsync(context);
            var requests = await SeedRequestsAsync(context);
            await SeedComplaintsAsync(context, companies, requests);
            await SeedFavoriteAddressesAsync(context);
            await SeedPollsAsync(context);
            await SeedPollOptionsAsync(context);
            await SeedPollVotesAsync(context);
            await SeedRequestCommentsAsync(context, requests);
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

    private static async Task<List<Company>> SeedCompaniesAsync(HooomeDbContext context)
    {
        if (await context.Companies.AnyAsync()) return await context.Companies.ToListAsync();

        var companies = new List<Company>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ОАО 'Минскжилстрой'",
                Phone = "+375 (17) 123-45-67",
                Email = "info@mzhs.by",
                WorkingHours = "8:30 - 17:30, Пн-Пт",
                Address = "г. Минск, ул. Берсона, 12",
                CreatedAt = DateTime.Now.AddDays(-30)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "УП 'Минсккоммунтеплосеть'",
                Phone = "+375 (17) 234-56-78",
                Email = "office@mkts.by",
                WorkingHours = "8:00 - 17:00, Пн-Пт",
                Address = "г. Минск, ул. Володарского, 24",
                CreatedAt = DateTime.Now.AddDays(-28)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "КУП 'ЖКХ Московского района г. Минска'",
                Phone = "+375 (17) 345-67-89",
                Email = "admin@jkhmosk.by",
                WorkingHours = "8:00 - 20:00, Пн-Сб",
                Address = "г. Минск, пр-т Дзержинского, 10",
                CreatedAt = DateTime.Now.AddDays(-25)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ООО 'Городские Лифты'",
                Phone = "+375 (17) 456-78-90",
                Email = "service@gorlift.by",
                WorkingHours = "Круглосуточно",
                Address = "г. Минск, ул. Тимирязева, 65",
                CreatedAt = DateTime.Now.AddDays(-22)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ГПО 'Минскгорсвет'",
                Phone = "+375 (17) 567-89-01",
                Email = "info@mgsvet.by",
                WorkingHours = "8:30 - 17:30, Пн-Пт",
                Address = "г. Минск, ул. Я. Коласа, 23",
                CreatedAt = DateTime.Now.AddDays(-20)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "УП 'Зеленстрой Советского района'",
                Phone = "+375 (17) 678-90-12",
                Email = "zelen@sovetsky.by",
                WorkingHours = "8:00 - 17:00, Пн-Пт",
                Address = "г. Минск, ул. Богдановича, 78",
                CreatedAt = DateTime.Now.AddDays(-18)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ОАО 'Брестжилстрой'",
                Phone = "+375 (162) 23-45-67",
                Email = "info@brestjs.by",
                WorkingHours = "8:30 - 17:30, Пн-Пт",
                Address = "г. Брест, ул. Гоголя, 34",
                CreatedAt = DateTime.Now.AddDays(-15)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "КУП 'Гомельское ЖКХ'",
                Phone = "+375 (232) 34-56-78",
                Email = "office@gomeljkx.by",
                WorkingHours = "8:00 - 17:00, Пн-Пт",
                Address = "г. Гомель, ул. Советская, 45",
                CreatedAt = DateTime.Now.AddDays(-12)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ООО 'ВитебскДорСтрой'",
                Phone = "+375 (212) 45-67-89",
                Email = "info@vitebskdor.by",
                WorkingHours = "8:30 - 17:30, Пн-Пт",
                Address = "г. Витебск, пр-т Строителей, 56",
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "УП 'Могилевводоканал'",
                Phone = "+375 (222) 56-78-90",
                Email = "office@mogilevvod.by",
                WorkingHours = "8:00 - 17:00, Пн-Пт",
                Address = "г. Могилев, ул. Первомайская, 67",
                CreatedAt = DateTime.Now.AddDays(-8)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Гродненское РУП 'ЖКХ'",
                Phone = "+375 (152) 67-89-01",
                Email = "info@grodnojkx.by",
                WorkingHours = "8:30 - 17:30, Пн-Пт",
                Address = "г. Гродно, ул. Ожешко, 78",
                CreatedAt = DateTime.Now.AddDays(-5)
            }
        };

        await context.Companies.AddRangeAsync(companies);
        await context.SaveChangesAsync();
        Log.Information($"Added {companies.Count} companies");

        return companies;
    }

    private static async Task<List<Request>> SeedRequestsAsync(HooomeDbContext context)
    {
        if (await context.Requests.AnyAsync()) return await context.Requests.ToListAsync();

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

        var addresses = new[]
        {
            "г. Минск, ул. Немига, 3, кв. 15",
            "г. Минск, пр-т Победителей, 23, кв. 48",
            "г. Минск, ул. Кальварийская, 12, кв. 7",
            "г. Минск, ул. Притыцкого, 56, кв. 92",
            "г. Минск, ул. Гурского, 34, кв. 23",
            "г. Минск, ул. Матусевича, 45, кв. 67",
            "г. Минск, ул. Рафиева, 78, кв. 12",
            "г. Минск, ул. Лобанка, 89, кв. 34",
            "г. Минск, ул. Шаранговича, 23, кв. 56",
            "г. Минск, ул. Алибегова, 67, кв. 78",
            "г. Минск, ул. Кунцевщина, 12, кв. 90",
            "г. Минск, ул. Горецкого, 34, кв. 23"
        };

        var requests = new List<Request>();
        var random = new Random();

        for (int i = 1; i <= 15; i++)
        {
            var category = categories[random.Next(categories.Length)];
            var status = statuses[random.Next(statuses.Length)];
            var createdDate = DateTime.Now.AddDays(-random.Next(1, 45));

            requests.Add(new Request
            {
                Id = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                Title = GetRequestTitle(category, i),
                Description = GetRequestDescription(category, i),
                Address = addresses[random.Next(addresses.Length)],
                Status = status,
                Category = category,
                PhotoUrl = random.Next(3) == 0 ? $"https://example.com/photos/request{i}.jpg" : string.Empty,
                CreatedAt = createdDate,
                UpdatedAt = status != RequestStatus.Created ? createdDate.AddDays(random.Next(1, 10)) : null
            });
        }

        await context.Requests.AddRangeAsync(requests);
        await context.SaveChangesAsync();
        Log.Information($"Added {requests.Count} requests");

        return requests;
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
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 30)),
                UpdatedAt = random.Next(2) == 0 ? DateTime.Now.AddDays(-random.Next(1, 15)) : null
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
                    Street = streets[random.Next(streets.Length)],
                    House = random.Next(1, 120),
                    Pseudonym = pseudonyms[random.Next(pseudonyms.Length)] + (j > 0 ? $" {j + 1}" : ""),
                    CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60)),
                    UpdatedAt = random.Next(2) == 0 ? DateTime.Now.AddDays(-random.Next(1, 30)) : null
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

        var polls = new List<Poll>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Качество уборки подъездов",
                Description = "Как вы оцениваете качество уборки в вашем подъезде?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-20)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Благоустройство дворовой территории",
                Description = "Что нужно улучшить в вашем дворе в первую очередь?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-18)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Работа управляющей компании",
                Description = "Оцените работу вашей управляющей компании за последний месяц",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-15)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Освещение во дворе",
                Description = "Достаточно ли освещен ваш двор в темное время суток?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-12)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Вывоз мусора",
                Description = "Устраивает ли вас график вывоза мусора?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-10)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Детские площадки",
                Description = "Нужна ли новая детская площадка в вашем дворе?",
                CreatedBy = Guid.NewGuid(),
                IsActive = false,
                CreatedAt = DateTime.Now.AddDays(-25)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Парковочные места",
                Description = "Как решить проблему с парковкой во дворе?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-8)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Озеленение территории",
                Description = "Какие деревья и кустарники вы хотели бы видеть во дворе?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-5)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Капитальный ремонт",
                Description = "Что нуждается в капитальном ремонте в первую очередь?",
                CreatedBy = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-3)
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Общественный транспорт",
                Description = "Устраивает ли вас работа общественного транспорта в районе?",
                CreatedBy = Guid.NewGuid(),
                IsActive = false,
                CreatedAt = DateTime.Now.AddDays(-30)
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
            var plannedStart = DateTime.Now.AddDays(random.Next(1, 30));
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
                Street = streets[random.Next(streets.Length)],
                House = random.Next(1, 100),
                Category = categories[random.Next(categories.Length)],
                Seriousness = seriousness,
                PlannedStartTime = plannedStart,
                PlannedEndTime = plannedStart.AddDays(random.Next(1, 14)),
                FactStartTime = random.Next(2) == 0 ? plannedStart.AddHours(-random.Next(1, 5)) : (DateTime?)null,
                FactEndTime = random.Next(2) == 0 ? plannedStart.AddDays(random.Next(1, 7)) : (DateTime?)null,
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 20)),
                UpdatedAt = random.Next(2) == 0 ? DateTime.Now.AddDays(-random.Next(1, 10)) : null
            });
        }

        await context.Works.AddRangeAsync(works);
        await context.SaveChangesAsync();
        Log.Information($"Added {works.Count} works");
    }

    // Вспомогательные методы
    private static string GetRequestTitle(RequestCategory category, int index)
    {
        return category switch
        {
            RequestCategory.HotWaterSupply => $"Отсутствует горячая вода в квартире {index}",
            RequestCategory.ColdWaterSupply => $"Холодная вода с ржавчиной",
            RequestCategory.PowerSupply => $"Отключение электричества в подъезде",
            RequestCategory.Heating => $"Холодные батареи в квартире",
            RequestCategory.ElevatorMaintenance => $"Сломался лифт в подъезде",
            RequestCategory.RoofingWorks => $"Течет крыша в районе подъезда",
            RequestCategory.Sewage => $"Засор канализации в подвале",
            RequestCategory.ApartmentBuildingSanitation => $"Мусор на лестничной клетке",
            RequestCategory.TerritorySanitation => $"Не вывозят мусор с контейнерной площадки",
            RequestCategory.StreetLighting => $"Не горит фонарь во дворе",
            RequestCategory.RoadsAndSidewalks => $"Яма на дороге возле дома",
            _ => $"Заявка №{index} по обслуживанию дома"
        };
    }

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