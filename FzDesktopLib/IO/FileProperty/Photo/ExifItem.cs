namespace FzLib.IO.FileProperty.Photo
{
    public class ExifItem
    {
        public string Title { get; protected internal set; }
        public string Description { get; protected internal set; }
        public int Id { get; protected internal set; }
        public int Length { get; protected internal set; }
        private object value;
        public object Value
        {
            get => value;
            protected internal set
            {
                this.value = value;
                NaturalLanguageValue = NaturalLanguageConverter.TryConvertValue(this);
            }
        }
        public DataType DataType { get; protected internal set; }
        public string NaturalLanguageValue { get; protected internal set; }
        public string ChineseTitle { get; protected internal set; }
        public IFD Ifd { get; protected internal set; }


        public override string ToString()
        {
            return string.Format("ID:{0}  \tTitle:{1}  \tValue:{2}  Type:{3}", Id, ChineseTitle ?? Title, NaturalLanguageValue ?? Value, DataType.ToString());
        }
    }
}
