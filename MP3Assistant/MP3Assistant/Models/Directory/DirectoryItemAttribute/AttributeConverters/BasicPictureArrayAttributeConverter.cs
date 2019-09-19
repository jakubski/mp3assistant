using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;

namespace MP3Assistant
{
    public class BasicPictureArrayAttributeConverter : IAttributeConverter
    {
        public object ForView(object passedValue)
        {
            if (passedValue == null)
                return new List<byte[]>();

            var pictureArray = (IPicture[])passedValue;
            var byteArrayList = pictureArray.Select(p => p.Data.Data).ToList();

            return byteArrayList;
        }

        public object FromView(object receivedValue)
        {
            if (receivedValue == null)
                return new Picture[] { };

            var byteArrayList = (List<byte[]>)receivedValue;
            var pictureArray = byteArrayList.Select(a => new Picture(new ByteVector(a))).ToArray();

            return pictureArray;
        }
    }
}
