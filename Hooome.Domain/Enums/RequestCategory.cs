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
    Other = 11,

    [Description("Техническое обслуживание лифта")]
    ElevatorMaintenance = 12,

    [Description("Обращение с ТКО")]
    SolidWasteManagement = 13,

    [Description("Водоснабжение. Холодная вода")]
    ColdWaterSupply = 14,

    [Description("Канализация")]
    Sewage = 15,

    [Description("Автомобильные дороги, тротуары")]
    RoadsAndSidewalks = 16,

    [Description("Кровельные работы")]
    RoofingWorks = 17,

    [Description("Уличное освещение")]
    StreetLighting = 18,

    [Description("Общественные места (Парки, скверы)")]
    PublicPlacesParksSquares = 19,

    [Description("Работы по ремонту стыков")]
    JointRepairWorks = 20,

    [Description("Техническое обслуживание зданий и сооружений")]
    BuildingMaintenance = 21,

    [Description("Рекламные и информационные конструкции и объявления")]
    AdvertisingAndInformationStructures = 22
}