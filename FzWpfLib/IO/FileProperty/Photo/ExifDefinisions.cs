namespace FzLib.IO.FileProperty.Photo
{
    internal enum ExifId
    {
        ImageTitle = 0x0320,
        EquipmentManufacturer = 0x010F,
        EquipmentModel = 0x0110,
        ExifDTOriginal = 0x9003,
        ExifExposureTime = 0x829A,
        LuminanceTable = 0x5090,
        ChrominanceTable = 0x5091
    }

    public enum DataType
    {
        Byte = 1,
        String = 2,
        UInt16 = 3,
        UInt32 = 4,
        URational = 5,
        Object = 6,
        Int32 = 7,
        Long = 9,
        Rational = 10
    }

    internal struct URational
    {
        public ulong Denominator;
        public ulong Numerator;
        public double DecimalValue => Denominator == 0 ? 0 : 1.0 * Numerator / Denominator;
        public override string ToString()
        {
            //return DecimalValue.ToString();
            return Numerator + "/" + Denominator;
        }
    }

    internal struct Rational
    {
        public long Denominator;
        public long Numerator;

        public double DecimalValue => Denominator==0?0: 1.0 * Numerator / Denominator;
        public override string ToString()
        {
            //return DecimalValue.ToString();
            return Numerator + "/" + Denominator;
        }
    }

   public enum IFD
    {
        Image,
        Photo,
        Iop,
        GPSInfo,
    }
}