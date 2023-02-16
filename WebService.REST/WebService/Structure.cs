using System.Collections.Generic;

public class Structure
{
    public class BUH
    {
        public string UID { get; set; }

        private string _ВидОбъекта;
        public string ВидОбъекта
        {
            set
            {
                if (value == "Документ_ВозвратТоваровОтПокупателя") { _ВидОбъекта = "Документ.ВозвратТовараПокупателем"; }
                else { _ВидОбъекта = value; }
            }
            get { return _ВидОбъекта; }
        }
        public string Транзакция { get; set; }
        public string Договор { get; set; }
        public string Договор_UID { get; set; }
        public string Контрагент { get; set; }
        public string Контрагент_УНП { get; set; }
      //  public string Контрагент_Код { get; set; }
        public string Склад { get; set; }
        public string Склад_UID { get; set; }
        private bool Склад_ОтветХранение { get; set; }
        public string ТорговаяТочка_КонтактнаяИнформация_Представление { get; set; }
        public int ФлЭлНакл { get; } = 1;
        public int АвтоЗаполнение { get; } = 0;
        public string ВидТМЦ { get; } = "Продукция";
        public string EDiN_эТТН_ID { get; set; }
        public string Дата { get; set; }
        public string ДатаДокВходящий { get { return Дата; } }

        private int _ВидВ;
        public int ВидВ
        {
            set
            {
                if (Склад_ОтветХранение == true) { _ВидВ = 2; }
                else { _ВидВ = 1; }
            }
            get { return _ВидВ; }
        }
        public List<ТабличнаяЧастьBUH> Номенклатура { get; set; }
    }


    public class ТабличнаяЧастьBUH
    {
        public string Номенклатура { get; set; }
        public int Количество { get; set; }
        public string Номенклатура_Код { get; set; }
        public int Уценка { get; set; }
        public float Цена { get; set; }
        public string СтавкаНДС { get; set; }
        public float Сумма { get; set; }
        public float СуммаНДС { get; set; }
        public float Всего { get; set; }
    }
}