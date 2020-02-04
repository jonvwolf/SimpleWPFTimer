using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SimpleTimer.IntegrationTests
{
    public class UtilTests
    {
        [Fact]
        public void Test_Utils_ItWorks()
        {
            //arrange
            string s = System.IO.Packaging.PackUriHelper.UriSchemePack;

            //act
            var stream = Utils.GetResourceStream(new ConfigurationValues().RingtoneFilename);

            //assert
            Assert.NotNull(stream);
            Assert.True(stream.Length > 0);
        }
    }
}
