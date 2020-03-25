using System;

namespace pCloudSDK
{
    public class pCloudException : Exception
    {
        public pCloudException(string errorMesage, int errorCode) : base(errorMesage) {}

    }

}
