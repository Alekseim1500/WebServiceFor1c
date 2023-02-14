using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public class CustomDataContractResolver : DefaultContractResolver
{
    public static readonly CustomDataContractResolver Instance = new CustomDataContractResolver();

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (property.DeclaringType == typeof(Structure.BUH))
        {
            if (property.PropertyName.Equals("Номенклатура", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "ТабличнаяЧасть"; }
            else if (property.PropertyName.Equals("ТорговаяТочка_КонтактнаяИнформация_Представление", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "ПунктРазгрузки"; }
            else if (property.PropertyName.Equals("Организация", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Контрагент"; }
            else if (property.PropertyName.Equals("Организация_УНП", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Контрагент_УНН"; }
            else if (property.PropertyName.Equals("Контрагент_Код", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Контрагент_GLN"; }
            else if (property.PropertyName.Equals("Дата", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "ДатаДок"; }
            else if (property.PropertyName.Equals("Склад", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "МестоХранения"; }
            else if (property.PropertyName.Equals("Склад_UID", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "МестоХранения_UID"; }
            else if (property.PropertyName.Equals("EDiN_эТТН_ID", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "НомерЭлНакл"; }
        }
        else if (property.DeclaringType == typeof(Structure.ТабличнаяЧастьBUH))
        {
            if (property.PropertyName.Equals("Номенклатура", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Товар"; }
            else if (property.PropertyName.Equals("СуммаНДС", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "НДС"; }
            else if (property.PropertyName.Equals("Уценка", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "ПроцентУценки"; }
            else if (property.PropertyName.Equals("Номенклатура_Код", StringComparison.OrdinalIgnoreCase)) { property.PropertyName = "Товар_GTIN"; }
        }
        return property;
    }
}