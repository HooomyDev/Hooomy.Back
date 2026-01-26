using System.ComponentModel;

namespace Hooome.Domain.Enums;

public enum RequestCategory
{
    [Description("Водоснабжение. Горячая вода")]
    HotWaterSupply,

    [Description("Электроснабжение")]
    PowerSupply,

    [Description("Бытовые услуги")]
    DomesticServices,

    [Description("Санитарное состояние многоквартирного дома")]
    ApartmentBuildingSanitation,

    [Description("Отопление")]
    Heating,

    [Description("Благоустройство территории")]
    TerritoryImprovement,

    [Description("Водоснабжение")]
    WaterSupply,

    [Description("Общестроительные работы")]
    GeneralConstruction,

    [Description("Санитарное состояние территории")]
    TerritorySanitation,

    [Description("Техническое обслуживание ЗПУ")]
    CivilDefenseShelterMaintenance,

    [Description("Другое")]
    Other,

    [Description("Техническое обслуживание лифта")]
    ElevatorMaintenance,

    [Description("Обращение с ТКО")]
    SolidWasteManagement,

    [Description("Водоснабжение. Холодная вода")]
    ColdWaterSupply,

    [Description("Канализация")]
    Sewage,

    [Description("Автомобильные дороги, тротуары")]
    RoadsAndSidewalks,

    [Description("Кровельные работы")]
    RoofingWorks,

    [Description("Уличное освещение")]
    StreetLighting,

    [Description("Общественные места (Парки, скверы)")]
    PublicPlacesParksSquares,

    [Description("Работы по ремонту стыков")]
    JointRepairWorks,

    [Description("Техническое обслуживание зданий и сооружений")]
    BuildingMaintenance,

    [Description("Рекламные и информационные конструкции и объявления")]
    AdvertisingAndInformationStructures
}