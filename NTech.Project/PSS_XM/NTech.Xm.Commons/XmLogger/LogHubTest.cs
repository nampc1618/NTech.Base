using NTech.Base.Commons.Logger;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTech.Xm.Commons.XmLogger
{
    [TestFixture]
    public class LogHubTest
    {
        [SetUp]
        public void SetUp()
        {
            //Attach NLogger to LogHub
            XmLogger nlogger = new XmLogger();            
            nlogger.GenerateLogger();
            LogHub.SetLogger(nlogger);
        }

        [Test]
        public void Write_Mode_InfoTest()
        {
            try
            {                
                LogHub.Write("Run to here", LogTypes.NTechInfo);
            }catch(Exception ex)
            {
                LogHub.Write(ex, "Error", LogTypes.NTechError);                
            }
            
        }
        [Test]
        public void Write_Mode_ErrorTest()
        {
            try
            {
                int val = 0;
                int result = 1 / val;
            }catch(Exception ex)
            {                
                LogHub.Write(ex, "Error", LogTypes.NTechError);
            }
            
        }
        [Test]
        public void Write_Mode_DebugTest()
        {
            try
            {
                int val = 0;
                LogHub.Write("value={0}, fix={1}", LogTypes.NTechDebug, val, 10);                
            }catch(Exception ex)
            {                
                LogHub.Write(ex, "Error", LogTypes.NTechError);
            }
            
        }
    }
}
