using System.IO;
using System.Text;

namespace rawdata
{
    internal class Program
    {
        public enum Format
        {
            RAW,

            INT8,
            UINT8,
            INT16,
            UINT16,
            INT32,
            UINT32,
            INT64,
            UINT64,

            DOUBLE32,
            DOUBLE64,
            FLOAT32,
            FLOAT64,

            ASCII,
            UTF32
        }

        public enum BytesOrder
        {
            NATIVE,
            BIG,
            LITTLE
        }

        internal Stream data;
        internal int size;//taille d'un élément en bytes
        internal int count;//nombre d'éléments consécutif ou -1 si EOF
        internal int offset;//offset jusqu'au premier element en bytes
        internal Format format;//format d'affichage/édition des données
        internal BytesOrder bytesOrder;//ordre des bytes
        internal byte[] currentData;//element actuel
        internal StringBuilder currentText;//element actuel

        public Program(Stream data, int size, Format format = Format.RAW, BytesOrder bytesOrder = BytesOrder.NATIVE, int offset = 0, int count = -1)
        {
            this.data = data;
            this.size = size;
            this.format = format;
            this.offset = offset;
            this.count = count;
            this.currentData = new byte[size];
            this.currentText = new StringBuilder();
        }

        public string Text
        {
            get
            {
                if (this.currentText.Length == 0)
                {
                    switch (format)
                    {
                        case Format.RAW:
                            this.currentText.Append(BitConverter.ToString(currentData, 0, currentData.Length));
                            break;
                    }
                    //conversion
                }
                return this.currentText.ToString();
            }
        }

        public bool ReadNext()
        {
            if (data.Length - data.Position < size || (count > 0 && (data.Position - offset) / size >= count))
                return false;
            data.ReadExactly(currentData, 0, size);
            currentText.Clear();
            return true;
        }

        public bool SeekTo(int n)
        {
            try
            {
                data.Seek(offset + (n * size), SeekOrigin.Begin);
            return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
