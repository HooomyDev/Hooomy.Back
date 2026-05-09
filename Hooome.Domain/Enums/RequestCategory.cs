using System.ComponentModel;

namespace Hooome.Domain.Enums;

public enum RequestCategory
{
    [Description("Все")]
    None = 0,

    [Description("Водоснабжение. Горячая вода")]
    HotWaterSupply = 1,

    [Description("Электроснабжение")]
    PowerSupply = 2,

    [Description("Бытовые услуги")]
    DomesticServices = 3,

    [Description("Санитарное состояние многоквартирного дома")]
    ApartmentBuildingSanitation = 4,

    [Description("Отопление")]
    Heating = 5,

    [Description("Благоустройство территории")]
    TerritoryImprovement = 6,

    [Description("Водоснабжение")]
    WaterSupply = 7,

    [Description("Общестроительные работы")]
    GeneralConstruction = 8,

    [Description("Санитарное состояние территории")]
    TerritorySanitation = 9,

    [Description("Техническое обслуживание ЗПУ")]
    CivilDefenseShelterMaintenance = 11,

    [Description("Другое")]
    Other = 12,

    [Description("Техническое обслуживание лифта")]
    ElevatorMaintenance = 13,

    [Description("Обращение с ТКО")]
    SolidWasteManagement = 14,

    [Description("Водоснабжение. Холодная вода")]
    ColdWaterSupply = 15,

    [Description("Канализация")]
    Sewage = 16,

    [Description("Автомобильные дороги, тротуары")]
    RoadsAndSidewalks = 17,

    [Description("Кровельные работы")]
    RoofingWorks = 18,

    [Description("Уличное освещение")]
    StreetLighting = 19,

    [Description("Общественные места (Парки, скверы)")]
    PublicPlacesParksSquares = 20,

    [Description("Работы по ремонту стыков")]
    JointRepairWorks = 21,

    [Description("Техническое обслуживание зданий и сооружений")]
    BuildingMaintenance = 22,

    [Description("Рекламные и информационные конструкции и объявления")]
    AdvertisingAndInformationStructures = 23
}