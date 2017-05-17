namespace BigInt.Controllers
{
    using System;
    using System.Numerics;
    using System.Globalization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class HomeController : Controller
    {
        private ILogger logger;

        public HomeController(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger(this.GetType().FullName);
        }

        public IActionResult Index()
        {
            logger.LogInformation("HomeController::Index()", null);

            return View();
        }

        public IActionResult Error()
        {
            logger.LogInformation("HomeController::Error()", null);

            return View();
        }

        // Dec to hex

        public IActionResult DecToHex()
        {
            logger.LogInformation("HomeController::DecToHex()", null);

            ViewBag.Input = 0;
            ViewBag.ResultVisibility = "collapse";

            return View();
        }

        public IActionResult DoDecToHex()
        {
            logger.LogInformation("CalculatorController::DoDecToHex()", null);

            try
            {
                ViewBag.Input = HttpContext.Request.Form["Input"].ToString();
                var decString = ViewBag.Input.Trim();

                var signed = decString.StartsWith("-");

                var bigint2 = BigInteger.Parse(decString, NumberStyles.Integer);
                BigInteger bigint1 = signed ? BigInteger.Parse(decString.Substring(1), NumberStyles.Integer) : bigint2;

                ViewBag.Result = bigint2.ToString("X");
                ViewBag.BitCount = bigint1.GetBitCount();
                ViewBag.ResultVisibility = "visible";
                return View("DecToHex");
            }
            catch (Exception ex)
            {
                ViewBag.Result = "Cannot convert: " + ex.Message;
                return View("Error");
            }
        }

        // Hex to dec

        public IActionResult HexToDec()
        {
            logger.LogInformation("HomeController::HexToDec()", null);

            ViewBag.Input = 0;
            ViewBag.ResultVisibility = "collapse";

            return View();
        }

        public IActionResult DoHexToDec()
        {
            logger.LogInformation("CalculatorController::DoDecToHex()", null);

            try
            {
                ViewBag.Input = HttpContext.Request.Form["Input"].ToString();
                var hexString = ViewBag.Input.Trim().ToLower();

                if (hexString.StartsWith("0x") || hexString.StartsWith("&h"))
                {
                    hexString = hexString.Substring(2);
                }

                while (hexString.StartsWith("0"))
                {
                    hexString = hexString.Substring(1);
                }

                BigInteger bigint1 = BigInteger.Parse("0" + hexString, NumberStyles.HexNumber);
                var bigint2 = BigInteger.Parse(hexString, NumberStyles.HexNumber);

                ViewBag.Result1 = bigint1.ToString("D");
                ViewBag.Result2 = bigint2.ToString("D");
                ViewBag.BitCount = bigint1.GetBitCount();
                ViewBag.ResultVisibility = "visible";
                return View("HexToDec");
            }
            catch (Exception ex)
            {
                ViewBag.Result = "Cannot convert: " + ex.Message;
                return View("Error");
            }
        }

        // Random

        public IActionResult Random()
        {
            logger.LogInformation("HomeController::Random()", null);

            ViewBag.BitCount = 128;
            ViewBag.ResultVisibility = "collapse";

            return View();
        }

        private Xorshift128Plus _random = new Xorshift128Plus();

        public IActionResult DoRandom()
        {
            logger.LogInformation("CalculatorController::DoRandom()", null);

            try
            {
                var input = HttpContext.Request.Form["BitCount"].ToString().ToLower();
                ViewBag.Input = input;

                var bitCount = Int32.Parse(input);

                BigInteger bigint = 0;
                var bigintBitCount = bigint.GetBitCount();
                while (bigintBitCount != bitCount)
                {
                    bigint = _random.Next(bitCount);
                    bigintBitCount = bigint.GetBitCount();
                }

                ViewBag.DecNumber = bigint.ToString("D");
                ViewBag.HexNumber = bigint.ToString("X");
                ViewBag.BitCount = bigint.GetBitCount();
                ViewBag.ResultVisibility = "visible";
                return View("Random");
            }
            catch (Exception ex)
            {
                ViewBag.Result = "Cannot convert: " + ex.Message;
                return View("Error");
            }
        }
    }
}
