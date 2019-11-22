using System;
using System.Collections.Generic;
using Euston.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Euston.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void WhenValidRegularEmailReturnNoErrors()
        {
            string messageBody = "email@sender.com \r\n"+
                                 "subject \r\n" +
                                 "This is regular email with url\r\n" +
                                 "https://www.google.com/";

            EmailProcessMessage email = new EmailProcessMessage();
            List<string> errors = new List<string>();
            int expectedErrors = 0;

            errors = email.RunValidation(messageBody);
            int errorsCount = errors.Count;

            Assert.AreEqual(expectedErrors, errorsCount);

        }

        [TestMethod]
        public void WhenValidSirEmailReturnNoErrors()
        {
            string messageBody = "email@sender.com \r\n" +
                                 "SIR 12/12/19 \r\n" +
                                 "66-666-99 \r\n" +
                                 "Staff Attack \r\n"+
                                 "This is regular email with url\r\n" +
                                 "https://www.google.com/";
  
            EmailProcessMessage email = new EmailProcessMessage();
            List<string> errors = new List<string>();
            int expectedErrors = 0;

            errors = email.RunValidation(messageBody);
            int errorsCount = errors.Count;

            Assert.AreEqual(expectedErrors, errorsCount);

        }

        [TestMethod]
        public void WhenInvalidEmailAddressReturnError()
        {
            string messageBody = "email.com \r\n" +
                                 "subject \r\n" +
                                 "This is regular email with url\r\n" +
                                 "https://www.google.com/";

            EmailProcessMessage email = new EmailProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("Invalid email address");

            errors = email.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);

        }

        [TestMethod]
        public void WhenInvalidSubjectReturnError()
        {
            string messageBody = "email@email.com \r\n" +
                                  "subjectThatHasOver20Characters \r\n" +
                                  "This is regular email with url\r\n" +
                                  "https://www.google.com/";

            EmailProcessMessage email = new EmailProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("Subject too long");

            errors = email.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);
        }

        [TestMethod]
        public void WhenInvalidMessageSizeReturnError()
        {
            string messageBody = "email@email.com \r\n" +
                                  "subject \r\n" +
                                  "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aliquam nisl nulla, consectetur non est auctor, molestie sodales risus. Nulla ut vehicula libero. Donec vitae ornare ipsum. Quisque sit amet egestas velit. Nam nec bibendum sem. Morbi egestas velit vel nibh faucibus porta. Nulla dolor lorem, scelerisque eu convallis nec, sollicitudin sit amet turpis. Aenean consequat tempor convallis. " +
                                  "Nulla lobortis diam sit amet nibh tempus malesuada. Pellentesque facilisis ornare ex nec molestie. Praesent rhoncus sem vel libero pretium aliquam ac ac mi. Nulla a mauris finibus, placerat tortor id, varius nulla.Aliquam pulvinar nibh a rutrum iaculis. Sed ultricies dictum mauris, sit amet vulputate est laoreet mattis.Phasellus suscipit ultrices turpis, vel mollis urna malesuada at. Ut sed tempus quam. Praesent pellentesque metus luctus, dapibus nulla at, rutrum eros.Mauris quis eleifend nisi. Nam quis justo eros." +
                                  "Donec eleifend quam odio, sed aliquet ipsum vulputate vitae. Praesent suscipit dolor non tincidunt scelerisque. Vestibulum et interdum leo. Vestibulum id felis viverra tortor maximus ornare.Sed molestie vitae odio a porta. Sed nisi dolor, tempor id velit et, dictum euismod leo. Praesent sed condimentum velit. Fusce eget tortor id velit convallis euismod.Nulla posuere urna eget magna tincidunt elementum.Sed laoreet lacinia cursus. In ut tortor eget sapien posuere condimentum et id dui.";
            

            EmailProcessMessage email = new EmailProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("Message too long");

            errors = email.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);
        }

        [TestMethod]
        public void WhenInvalidSirEmailDateReturnError()
        {
            string messageBody = "email@sender.com \r\n" +
                                 "SIR 12/12 /190 \r\n" +
                                 "66-666-99 \r\n" +
                                 "Staff Attack \r\n" +
                                 "This is regular email with url\r\n" +
                                 "https://www.google.com/";

            EmailProcessMessage email = new EmailProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("Invalid date format");

            errors = email.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);

        }

        [TestMethod]
        public void WhenInvalidSirEmailCodeReturnError()
        {
            string messageBody = "email@sender.com \r\n" +
                                 "SIR 12/12/19 \r\n" +
                                 "66-66-99 \r\n" +
                                 "Staff Attack \r\n" +
                                 "This is regular email with url\r\n" +
                                 "https://www.google.com/";

            EmailProcessMessage email = new EmailProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("Invalid center code");

            errors = email.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);

        }
        [TestMethod]
        public void WhenInvalidSirEmailIncidentReturnError()
        {
            string messageBody = "email@sender.com \r\n" +
                                 "SIR 12/12/19 \r\n" +
                                 "66-667-99 \r\n" +
                                 "Staff \r\n" +
                                 "This is regular email with url\r\n" +
                                 "https://www.google.com/";

            EmailProcessMessage email = new EmailProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("Invalid Nature of Incident");

            errors = email.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);

        }

        [TestMethod]
        public void WhenValidSMSReturnNoErrors()
        {
            string messageBody = "00447894561231\r\n" +
                                 "Saw your message ROTFL can’t wait to see you \r\n";

            SMSProcessMessage sms = new SMSProcessMessage();
            List<string> errors = new List<string>();
            int expectedErrors = 0;

            errors = sms.RunValidation(messageBody);
            int errorsCount = errors.Count;

            Assert.AreEqual(expectedErrors, errorsCount);

        }


        [TestMethod]
        public void WhenInvalidSMSLengthReturnError()
        {
            string messageBody = "00447894561231\r\n" +
                                 "Saw your message ROTFL can’t wait to see you \r\n"+
                                 "Saw your message ROTFL can’t wait to see you  \r\n" +
                                 "Saw your message ROTFL can’t wait to see you  \r\n" +
                                 "Saw your message ROTFL can’t wait to see you  \r\n";

            SMSProcessMessage sms = new SMSProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("Message too long");

            errors = sms.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);

        }

        [TestMethod]
        public void WhenInvalidSMSNumberReturnError()
        {
            string messageBody = "07894561231 \r\n" +
                                "Saw your message ROTFL can’t wait to see you \r\n";

            SMSProcessMessage sms = new SMSProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("Invalid phone number");

            errors = sms.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);

        }

        [TestMethod]
        public void WhenValidTweetReturnNoErrors()
        {
            string messageBody = "@twitter1\r\n" +
                                 "We welcomed #TweeParents to Twitter HQ for our first-ever Bring Your Parents to Work Day yesterday. \r\n " +
                                 "B4N @twitter2";

            TweetProcessMessage tweet = new TweetProcessMessage();
            List<string> errors = new List<string>();
            int expectedErrors = 0;

            errors = tweet.RunValidation(messageBody);
            int errorsCount = errors.Count;

            Assert.AreEqual(expectedErrors, errorsCount);

        }

        [TestMethod]
        public void WhenInvalidTwitterIDReturnError()
        {
            string messageBody = "@toolongtwitterID \r\n" +
                                 "We welcomed #TweeParents to Twitter HQ for our first-ever Bring Your Parents to Work Day yesterday. \r\n " +
                                 "B4N @twitter2";

            TweetProcessMessage tweet = new TweetProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("TwittedID is too long");

            errors = tweet.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);

        }

        [TestMethod]
        public void WhenInvalidTwitterLengthReturnError()
        {
            string messageBody = "@twitterID \r\n" +
                                 "We welcomed #TweeParents to Twitter HQ for our first-ever Bring Your Parents to Work Day yesterday. \r\n " +
                                 "We welcomed #TweeParents to Twitter HQ for our first-ever Bring Your Parents to Work Day yesterday. B4N @twitter2";

            TweetProcessMessage tweet = new TweetProcessMessage();
            List<string> errors = new List<string>();
            List<string> expectedErrors = new List<string>();
            expectedErrors.Add("Message too long");

            errors = tweet.RunValidation(messageBody);

            Assert.AreEqual(expectedErrors[0], errors[0]);

        }
    }
}
