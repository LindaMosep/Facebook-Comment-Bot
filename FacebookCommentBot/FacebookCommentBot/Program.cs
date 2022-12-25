using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Diagnostics;
using Console = ConsoleLibrary.Console;

internal static class Program
{
    public static string SearchWord = "gaming";
    public static int imageIndex = 0;
    public static int textIndex = 0;
    public static int delayBetweenProfiles = 1;
    public static int ProfileIndex = 0;
    public static bool isHeadless = true;
    public static void Main()
    {
        System.Console.WriteLine("=================== Facebook Comment/Like Bot V1.06 - Made by LindaMosep ===================");
        var lines = File.ReadAllLines(Environment.CurrentDirectory + "/Config.txt").ToList().FindAll(m => m.Trim().Length > 0 && m != String.Empty);

        SearchWord = lines[0].Remove(0, "SearchWord:".Length).Trim();
        System.Console.WriteLine($"Search word: {SearchWord}");

        if (int.TryParse(lines[1].Remove(0, "MinuteDelayBetweenComments:".Length).Trim(), out int time))
        {
            delayBetweenProfiles = time;

            System.Console.WriteLine($"Minute delay between comments: {delayBetweenProfiles}");
           
        }
        else
        {
            System.Console.WriteLine($"You did not input valid number for minute delay between comments, please input valid number. I'm gonna stop program in 10 seconds.");
            Task.Delay(10000).Wait();
            Environment.Exit(0);
        }

        if (lines.Count > 2)
        {
            if (bool.TryParse(lines[2].Remove(0, "ChromeVisible:".Length).Trim(), out bool res))
            {
                isHeadless = res;
                System.Console.WriteLine($"Chrome Visibility: {res}");
                System.Console.WriteLine($"============================================================================================");

            }
            else
            {
                System.Console.WriteLine($"You did not input valid value to Chrome Visibility, please just input 'true' or 'false'. I'm gonna stop program in 10 seconds.");
                System.Console.WriteLine($"============================================================================================");
                Task.Delay(10000).Wait();
                Environment.Exit(0);
            }
        }
        else
        {
            System.Console.WriteLine($"Please input this lines to under of Config file: ChromeVisible:true\nI'm gonna stop program in 10 seconds.");
            System.Console.WriteLine($"============================================================================================");
            Task.Delay(10000).Wait();
            Environment.Exit(0);

        }



        MainAsync().Wait();
    }

    public static async Task MainAsync()
    {
        LoadConfig();
        CommentForAcc();
        await Task.Delay(-1);
    }

    public static async Task LoadConfig()
    {
        for (int i = 0; true;)
        {
            var lines = File.ReadAllLines(Environment.CurrentDirectory + "/Config.txt").ToList().FindAll(m => m.Trim().Length > 0 && m != String.Empty);

            if (SearchWord != lines[0].Remove(0, "SearchWord:".Length).Trim())
            {
                SearchWord = lines[0].Remove(0, "SearchWord:".Length).Trim();
                Console.WriteLine($"Search word changed to {lines[0].Remove(0, "SearchWord:".Length).Trim()}");
            }

            if (int.TryParse(lines[1].Remove(0, "MinuteDelayBetweenComments:".Length).Trim(), out int time))
            {
                if (delayBetweenProfiles != time)
                {
                    Console.WriteLine($"Minute delay between comments changed to {time}");
                }

                delayBetweenProfiles = time;


            }
            else
            {

            }

            await Task.Delay(1000);
        }


    }

    public static async Task CommentForAcc()
    {
        var files = Directory.GetDirectories(Environment.CurrentDirectory + "/Profiles");
        var profiles = new List<string>();
        foreach (var file in files)
        {
            if (file.Contains("Profile "))
            {
                profiles.Add(file);
            }
        }
        Console.WriteLine("Profiles Count: " + profiles.Count);
        for (int testetest = 0; true;)
        {
            await CommentLoop(profiles);
        }



    }

    public static async Task CommentLoop(List<string> profiles)
    {
        for (int profileSelection = 0; profileSelection < profiles.Count; profileSelection++)
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("chromedriver"))
                {

                    proc.Kill();
                }

                // foreach (Process proc in Process.GetProcessesByName("chrome"))
                // {
                //
                //     proc.Kill();
                // }

                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--ignore-certificate-errors");
               
                if (isHeadless == false)
                {
                  options.AddArguments("--headless=chrome", "--disable-gpu", "--ignore-certificate-errors", "--disable-extensions", "--no-sandbox", "--disable-dev-shm-usage");
                }
                else
                {
                    options.AddArguments("--disable-gpu", "--ignore-certificate-errors", "--disable-extensions", "--no-sandbox", "--disable-dev-shm-usage");
                }
                var cap = options.ToCapabilities();



                options.AddArgument("--remote-debugging-port=45447");


                options.AddArgument($"user-data-dir={Environment.CurrentDirectory + "/Profiles"}");

                for (int i = 0; i < profiles.Count; i++)
                {

                    profiles[i] = Path.GetFileName(profiles[i]);
                }

                var profileName = profiles[profileSelection];

                options.AddArgument("profile-directory=" + profileName);
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.SuppressInitialDiagnosticInformation = true;
                service.HideCommandPromptWindow = true;
                if (Directory.GetFiles(Environment.CurrentDirectory + "/Images").Length == 0)
                {
                    Console.WriteLine($"You did not input any image to 'Images' folder, please input some images.");
                }
                while (Directory.GetFiles(Environment.CurrentDirectory + "/Images").Length == 0)
                {
                    await Task.Delay(1);
                }
                if (Directory.GetFiles(Environment.CurrentDirectory + "/Images").Length > 0)
                {

                    var images = Directory.GetFiles(Environment.CurrentDirectory + "/Images");
                    var texts = File.ReadAllLines(Environment.CurrentDirectory + "/Texts.txt").ToList().FindAll(m => m.Trim().Length > 0 && m != String.Empty);
                    var imagePath = "";
                    var textString = "Please, check this out! It's amazing product!";

                    if (images.ToList().Count > imageIndex)
                    {
                        imagePath = images[imageIndex];
                        imageIndex++;
                    }
                    else
                    {
                        imageIndex = 0;
                        imagePath = images[0];
                        imageIndex++;
                    }

                    if (texts.Count > textIndex)
                    {
                        textString = texts[textIndex];
                        textIndex++;
                    }
                    else
                    {

                        textIndex = 0;
                        textString  = texts[textIndex];
                        textIndex++;
                    }


                    using (var driver = new ChromeDriver(service, options))
                    {

                        driver.Manage().Window.Maximize();
                        driver.Navigate().GoToUrl("https://facebook.com/me");
                        var executer = (IJavaScriptExecutor)driver;
                        var con = true;

                        #region Page Load

                        for (int waitUntil = 0; true; waitUntil++)
                        {
                            if (executer.ExecuteScript("return document.readyState").ToString().ToLower() == "complete")
                            {
                                break;
                            }
                            else if (waitUntil >= 60)
                            {
                                con = false;
                                break;
                            }
                            await Task.Delay(100);
                        }

                        #endregion

                        if (con)
                        {
                            await Task.Delay(1000); // Change Delay

                            var profileUrl = driver.Url;
                            if (profileUrl.EndsWith("/"))
                            {
                                profileUrl = profileUrl.Remove(profileUrl.Length - 1);
                            }
                            var res = executer.ExecuteScript("if(document.getElementById(\"email\") == null){return 0;}else{return null;}");

                            if (res != null)
                            {
                                Console.WriteLine("Starting comment section for the " + profileName);
                                driver.Navigate().GoToUrl("https://www.facebook.com/search/top?q=" + Uri.EscapeDataString(SearchWord));

                                #region Page Load
                                for (int waitUntil = 0; true; waitUntil++)
                                {
                                    if (executer.ExecuteScript("return document.readyState").ToString().ToLower() == "complete")
                                    {
                                        break;
                                    }
                                    else if (waitUntil >= 60)
                                    {
                                        con = false;
                                        break;
                                    }

                                    await Task.Delay(200);
                                }
                                #endregion

                                await Task.Delay(1000); // Change Delay
                                if (con)
                                {
                                    bool isCommented = false;

                                    List<IWebElement> totalAds = new List<IWebElement>();

                                    List<IWebElement> totalComments = new List<IWebElement>();
                                    int commentCount = 0;
                                    int imageShareButtonCount = 0;
                                    List<int> teles = new List<int>();
                                    for (int scroll = 0; scroll < 5000; scroll++)
                                    {

                                        #region Progress Bar Load

                                        var progressBar = (Int64)executer.ExecuteScript("let array =Array.from(document.getElementsByClassName(\"x1yztbdb\")).filter(m => m.className == \"x1yztbdb\" && m.role == \"article\").filter(m => \n    {\n       \n\n        if(m.childNodes.length > 0)\n        {\n           \n            if(m.childNodes[0].className == \"x78zum5 x1n2onr6 xh8yej3\")\n            {\n                if(m.childNodes[0].childNodes.length > 0)\n                {\n                   if(m.childNodes[0].querySelectorAll(\"div[role=\'progressbar\']\").length > 0)\n                   {\n                         return true;\n                   }else\n                   {\n                       return false;\n                   }\n                  \n                   \n                }else\n                {\n                    return false;\n                }\n              \n            }else\n                {\n                    return false;\n                }\n        }else\n                {\n                    return false;\n                }\n       \n    });\n\nreturn array.length;");
                                        for (int progress = 0; progress < 15; progress++)
                                        {

                                            if (progressBar == 0)
                                            {

                                                break;
                                            }
                                            else
                                            {
                                                await Task.Delay(500);
                                                progressBar = (Int64)executer.ExecuteScript("let array =Array.from(document.getElementsByClassName(\"x1yztbdb\")).filter(m => m.className == \"x1yztbdb\" && m.role == \"article\").filter(m => \n    {\n       \n\n        if(m.childNodes.length > 0)\n        {\n           \n            if(m.childNodes[0].className == \"x78zum5 x1n2onr6 xh8yej3\")\n            {\n                if(m.childNodes[0].childNodes.length > 0)\n                {\n                   if(m.childNodes[0].querySelectorAll(\"div[role=\'progressbar\']\").length > 0)\n                   {\n                         return true;\n                   }else\n                   {\n                       return false;\n                   }\n                  \n                   \n                }else\n                {\n                    return false;\n                }\n              \n            }else\n                {\n                    return false;\n                }\n        }else\n                {\n                    return false;\n                }\n       \n    });\n\nreturn array.length;");
                                            }
                                            if (progress == 14)
                                            {

                                            }
                                        }

                                        #endregion

                                        #region Get Posts

                                        res = executer.ExecuteScript("try\n{var elems = new Array();\nvar xpaths = new Array();\nArray.from( document.getElementsByClassName(\"xmper1u xt0psk2 xjb2p0i x1qlqyl8 x15bjb6t x1n2onr6 x17ihmo5 x1g77sc7\")).filter(m => m.className == \"xmper1u xt0psk2 xjb2p0i x1qlqyl8 x15bjb6t x1n2onr6 x17ihmo5 x1g77sc7\").filter(m => m.parentNode != null).forEach(addToElem);\n\nfunction addToElem(item)\n{\n   \n    try\n    {\n        elems.push(item.parentNode.parentNode);\n        \n    }catch(err)\n    {\n       \n    }\n}\n\nelems = elems.filter(m => m.className.length == 0);\nfunction elemCheck(item)\n{\n  var ct = item.querySelectorAll(\"div[aria-label=\'Leave a comment\']\");\n\n   \nArray.from(ct).forEach((m) => \n    {\n\n        try\n        {  \n          xpaths.push(m);\n            \n        }catch(err)\n        {\n            console.log(err.message);\n        }\n       \n    });\n}\n\nfunction getSelector(elm)\n{\nif (elm.tagName === \"BODY\") return \"BODY\";\nconst names = [];\nwhile (elm.parentElement && elm.tagName !== \"BODY\") {\n    if (elm.id) {\n        names.unshift(\"#\" + elm.getAttribute(\"id\")); // getAttribute, because `elm.id` could also return a child element with name \"id\"\n        break; // Because ID should be unique, no more is needed. Remove the break, if you always want a full path.\n    } else {\n        let c = 1, e = elm;\n        for (; e.previousElementSibling; e = e.previousElementSibling, c++) ;\n        names.unshift(elm.tagName + \":nth-child(\" + c + \")\");\n    }\n    elm = elm.parentElement;\n}\n \nreturn names.join(\">\");\n}\n\nvar secElems = new Array();\n\nfunction addNew(item)\n{\n    try\n    {\n        \n        secElems.push(item.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode);\n    }catch(err)\n    {\n        console.log(err.message);\n    }\n}\n\n\nelems.forEach(addNew);\nconsole.log(secElems.length);\nsecElems.forEach(elemCheck);\n\n\nfunction GetPath(el) {\n    if (!(el instanceof Element)) return null;\n    var path = [];\n    while (el.nodeType === Node.ELEMENT_NODE) {\n        var selector = el.nodeName.toLowerCase();\n        if (el.id) {\n            selector += \'#\' + el.id;\n        } else {\n            var sib = el, nth = 1;\n            while (sib.nodeType === Node.ELEMENT_NODE && (sib = sib.previousSibling) && nth++);\n            selector += \":nth-child(\"+nth+\")\";\n        }\n        path.unshift(selector);\n        el = el.parentNode;\n    }\n    return path.join(\" > \");\n}\n\nfunction createXPathFromElement(elm) { \n    var allNodes = document.getElementsByTagName(\'*\'); \n    for (segs = []; elm && elm.nodeType == 1; elm = elm.parentNode) \n    { \n        if (elm.hasAttribute(\'id\')) { \n                var uniqueIdCount = 0; \n                for (var n=0;n < allNodes.length;n++) { \n                    if (allNodes[n].hasAttribute(\'id\') && allNodes[n].id == elm.id) uniqueIdCount++; \n                    if (uniqueIdCount > 1) break; \n                }; \n                if ( uniqueIdCount == 1) { \n                    segs.unshift(\'id(\"\' + elm.getAttribute(\'id\') + \'\")\'); \n                    return segs.join(\'/\'); \n                } else { \n                    segs.unshift(elm.localName.toLowerCase() + \'[@id=\"\' + elm.getAttribute(\'id\') + \'\"]\'); \n                } \n        } else if (elm.hasAttribute(\'class\')) { \n            segs.unshift(elm.localName.toLowerCase() + \'[@class=\"\' + elm.getAttribute(\'class\') + \'\"]\'); \n        } else { \n            for (i = 1, sib = elm.previousSibling; sib; sib = sib.previousSibling) { \n                if (sib.localName == elm.localName)  i++; }; \n                segs.unshift(elm.localName.toLowerCase() + \'[\' + i + \']\'); \n        }; \n    }; \n    return segs.length ? \'/\' + segs.join(\'/\') : null; \n}; \n\nreturn xpaths;\n    \n}catch(err)\n{return err.message};\n");

                                        #endregion

                                        if (res != null)
                                        {

                                            if (res is System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>)
                                            {
                                                var adsElements = ((System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>)res).ToList();

                                                List<IWebElement> adsRealList = new List<IWebElement>();

                                                res = executer.ExecuteScript("var elems = new Array();\nvar paths = new Array();\nArray.from( document.getElementsByClassName(\"xmper1u xt0psk2 xjb2p0i x1qlqyl8 x15bjb6t x1n2onr6 x17ihmo5 x1g77sc7\")).filter(m => m.className == \"xmper1u xt0psk2 xjb2p0i x1qlqyl8 x15bjb6t x1n2onr6 x17ihmo5 x1g77sc7\").filter(m => m.parentNode != null).forEach(addToElem);\n\nfunction addToElem(item)\n{\n   \n    try\n    {\n        elems.push(item.parentNode.parentNode);\n        \n    }catch(err)\n    {\n       \n    }\n}\n\nelems = elems.filter(m => m.className.length == 0);\nfunction elemCheck(item)\n{\n  var ct = item.querySelectorAll(\"div[aria-label=\'Leave a comment\']\");\n\n   \nArray.from(ct).forEach((m) => \n    {\n       paths.push(item);\n        \n    });\n}\n\n function GetSelector(el) {\n      if (el.tagName.toLowerCase() == \"html\")\n          return \"HTML\";\n      var str = el.tagName;\n      str += (el.id != \"\") ? \"#\" + el.id : \"\";\n      if (el.className) {\n          var classes = el.className.split(/\\s/);\n          for (var i = 0; i < classes.length; i++) {\n              str += \".\" + classes[i]\n          }\n      }\n      return el.parentNode + \" > \" + str;\n}\n\nvar secElems = new Array();\n\nfunction addNew(item)\n{\n    try\n    {\n        secElems.push(item.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode.parentNode);\n    }catch(err)\n    {\n        console.log(err.message);\n    }\n}\n\n\nelems.forEach(addNew);\nconsole.log(secElems.length);\nsecElems.forEach(elemCheck);\n\n\nfunction GetPath(el) {\n    if (!(el instanceof Element)) return null;\n    var path = [];\n    while (el.nodeType === Node.ELEMENT_NODE) {\n        var selector = el.nodeName.toLowerCase();\n        if (el.id) {\n            selector += \'#\' + el.id;\n        } else {\n            var sib = el, nth = 1;\n            while (sib.nodeType === Node.ELEMENT_NODE && (sib = sib.previousSibling) && nth++);\n            selector += \":nth-child(\"+nth+\")\";\n        }\n        path.unshift(selector);\n        el = el.parentNode;\n    }\n    return path.join(\" > \");\n}\n\nreturn paths;");

                                                if (res is System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>)
                                                {
                                                    adsRealList.Clear();
                                                    adsRealList.AddRange((System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>)res);
                                                }

                                                await Task.Delay(4000);

                                                if (adsElements.Count > 0)
                                                {

                                                    for (int tele = totalAds.Count; tele < adsElements.Count; tele++)
                                                    {
                                                        var m = adsElements[tele];
                                                        executer.ExecuteScript("arguments[0].scrollIntoView(true);", m);


                                                        #region Page Load
                                                        for (int waitUntil = 0; true; waitUntil++)
                                                        {
                                                            if (executer.ExecuteScript("return document.readyState").ToString().ToLower() == "complete")
                                                            {
                                                                break;
                                                            }
                                                            else if (waitUntil >= 60)
                                                            {
                                                                con = false;
                                                                break;
                                                            }

                                                            await Task.Delay(200);
                                                        }
                                                        #endregion


                                                        Actions actions = new Actions(driver);
                                                        if (con)
                                                        {
                                                            // res = executer.ExecuteScript("const queryName = " + "\"" + m + "\";" + "if(document.querySelector(queryName) != null)\n{\n    try\n    { \n        document.querySelector(queryName).click();\n        return true;\n        \n    }catch(err)\n    {\n        \n        return err;\n    }\n   \n}else\n{\n    return false;\n}");

                                                            executer.ExecuteScript("arguments[0].click();", m);


                                                            res = true;
                                                            if (res is bool)
                                                            {

                                                                if ((bool)res == true)
                                                                {
                                                                    progressBar = (Int64)executer.ExecuteScript("let array =Array.from(document.getElementsByClassName(\"x1yztbdb\")).filter(m => m.className == \"x1yztbdb\" && m.role == \"article\").filter(m => \n    {\n       \n\n        if(m.childNodes.length > 0)\n        {\n           \n            if(m.childNodes[0].className == \"x78zum5 x1n2onr6 xh8yej3\")\n            {\n                if(m.childNodes[0].childNodes.length > 0)\n                {\n                   if(m.childNodes[0].querySelectorAll(\"div[role=\'progressbar\']\").length > 0)\n                   {\n                         return true;\n                   }else\n                   {\n                       return false;\n                   }\n                  \n                   \n                }else\n                {\n                    return false;\n                }\n              \n            }else\n                {\n                    return false;\n                }\n        }else\n                {\n                    return false;\n                }\n       \n    });\n\nreturn array.length;");
                                                                    for (int progress = 0; progress < 15; progress++)
                                                                    {

                                                                        if (progressBar == 0)
                                                                        {

                                                                            break;
                                                                        }
                                                                        else
                                                                        {
                                                                            await Task.Delay(500);
                                                                            progressBar = (Int64)executer.ExecuteScript("let array =Array.from(document.getElementsByClassName(\"x1yztbdb\")).filter(m => m.className == \"x1yztbdb\" && m.role == \"article\").filter(m => \n    {\n       \n\n        if(m.childNodes.length > 0)\n        {\n           \n            if(m.childNodes[0].className == \"x78zum5 x1n2onr6 xh8yej3\")\n            {\n                if(m.childNodes[0].childNodes.length > 0)\n                {\n                   if(m.childNodes[0].querySelectorAll(\"div[role=\'progressbar\']\").length > 0)\n                   {\n                         return true;\n                   }else\n                   {\n                       return false;\n                   }\n                  \n                   \n                }else\n                {\n                    return false;\n                }\n              \n            }else\n                {\n                    return false;\n                }\n        }else\n                {\n                    return false;\n                }\n       \n    });\n\nreturn array.length;");
                                                                        }
                                                                        if (progress == 14)
                                                                        {

                                                                        }
                                                                    }

                                                                    await Task.Delay(2000); // Change Delay

                                                                    #region Checking Comments

                                                                    var comments = new List<IWebElement>();

                                                                    var comObjects = executer.ExecuteScript("try\n{\n    \n    \n console.log(document.querySelectorAll(\"div[aria-label*=\'Comment by \']\"));  return document.querySelectorAll(\"div[aria-label*=\'Comment by \']\");\n\n   \n  \n}catch(err)\n{\n   console.log(err.message);\n}\n\n");

                                                                    if (comObjects is System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>)
                                                                    {
                                                                        comments.AddRange((System.Collections.ObjectModel.ReadOnlyCollection<IWebElement>)comObjects);
                                                                    }

                                                                    foreach (var comment in totalComments)
                                                                    {
                                                                        bool mothefucka = comments.Remove(comment);

                                                                    }

                                                                    totalComments.AddRange(comments);

                                                                    var commentUrls = new List<int>();

                                                                    if (comments.Count > 0)
                                                                    {

                                                                        foreach (var comment in comments)
                                                                        {
                                                                            var urlsObj = executer.ExecuteScript("try\n{\n  console.log(arguments[0].querySelectorAll(\"[href*=\'\" + String(arguments[1]))); console.log(arguments[0]); console.log(arguments[1]);  \n    \n  return arguments[0].querySelectorAll(\"[href*=\'\" + String(arguments[1])).length;\n\n\n  \n}catch(err)\n{\n return null; \n}\n\n", comment, profileUrl);

                                                                            if (urlsObj != null)
                                                                            {


                                                                                if (Convert.ToInt64(urlsObj) > 0)
                                                                                {
                                                                                    commentUrls.Add(1);
                                                                                }
                                                                            }

                                                                        }
                                                                    }

                                                                    commentUrls = commentUrls.Distinct().ToList();

                                                                    #endregion

                                                                    #region Sending Comment

                                                                    if (commentUrls.Count == 0)
                                                                    {

                                                                        if (m.FindElements(By.XPath("//input[contains(@class, 'x1s85apg')]")).Count - imageShareButtonCount > 0)
                                                                        {

                                                                            m.FindElements(By.XPath("//input[contains(@class, 'x1s85apg')]")).Last().SendKeys(imagePath);

                                                                            bool isShared = false;

                                                                            for (int i = 0; true; i++)
                                                                            {
                                                                                var isMore = m.FindElements(By.XPath("//div[contains(text(), 'The photo was successfully attache')][contains(@aria-live, 'polite')][contains(@role, 'status')]")).Count;

                                                                                if (isMore != null)
                                                                                {
                                                                                    if (isMore is int)
                                                                                    {
                                                                                        if ((int)isMore == 1)
                                                                                        {
                                                                                            isShared = true;

                                                                                            break;
                                                                                        }
                                                                                        else
                                                                                        {

                                                                                            isShared = false;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {

                                                                                    isShared = false;
                                                                                }

                                                                                if (i >= 45)
                                                                                {
                                                                                    break;
                                                                                }

                                                                                if (!teles.Contains(i))
                                                                                {
                                                                                    teles.Add(i);
                                                                                    imageShareButtonCount++;
                                                                                }

                                                                                await Task.Delay(300);


                                                                            }

                                                                            if (isShared)
                                                                            {
                                                                                await Task.Delay(1000);
                                                                                var keyboard = m.FindElements(By.XPath("//div[contains(@role, 'textbox')][contains(@contenteditable, 'true')]"));
                                                                                if (keyboard.Count > 0)
                                                                                {
                                                                                    keyboard.Last().SendKeys(textString);
                                                                                    await Task.Delay(1000);
                                                                                    keyboard.Last().SendKeys(Keys.Enter);
                                                                                    keyboard.Last().SendKeys(Keys.Enter);
                                                                                    isCommented = true;
                                                                                    if (adsRealList.Count > tele)
                                                                                    {


                                                                                        executer.ExecuteScript("try\r\n{\r\n    var ct = arguments[0].querySelectorAll(\"span[class=\'x193iq5w xeuugli x13faqbe x1vvkbs x1xmvt09 x1lliihq x1s928wv xhkezso x1gmr53x x1cpjm7i x1fgarty x1943h6x xudqn12 x3x7a5m x6prxxf xvq8zen x1s688f xi81zsa\']\");\r\nArray.from(ct).forEach((m) => \r\n    {\r\n\r\n        \r\n        if(m.innerText == \'Like\')\r\n        {\r\n             m.click();\r\n        }\r\n      \r\n    });\r\n \r\n}catch(err)\r\n{\r\n console.log(err.message);   \r\n}", adsRealList[tele]);
                                                                                        await Task.Delay(2000);
                                                                                    }
                                                                                    else
                                                                                    {

                                                                                    }
                                                                                    executer.ExecuteScript("window.scrollBy(0, -450);", keyboard.Last());
                                                                                    await Task.Delay(8000);



                                                                                    var ss = driver.GetScreenshot();

                                                                                    ss.SaveAsFile(Environment.CurrentDirectory + $"/Comments/{profileName} - {DateTime.UtcNow.Ticks}.png", ScreenshotImageFormat.Png);

                                                                                    break;
                                                                                }
                                                                                else
                                                                                {

                                                                                }
                                                                            }


                                                                        }
                                                                        else
                                                                        {

                                                                        }
                                                                    }
                                                                    else
                                                                    {

                                                                        commentCount++;
                                                                        await Task.Delay(1000);
                                                                    }

                                                                    #endregion
                                                                }
                                                                else
                                                                {

                                                                }
                                                            }
                                                            else if (res is string)
                                                            {
                                                                Console.WriteLine("Error: " + res.ToString());
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Error - 2: " + res.ToString());
                                                            }

                                                        }
                                                        executer.ExecuteScript("arguments[0].click();", m);
                                                    }


                                                }

                                                totalAds.Clear();
                                                totalAds.AddRange(adsElements);


                                            }
                                        }

                                        var scrollEndObj = executer.ExecuteScript("return  document.getElementsByClassName(\"x1n2onr6 x1ja2u2z x9f619 x78zum5 xdt5ytf x2lah0s x193iq5w xz9dl7a\").length;");
                                        if (scrollEndObj != null)
                                        {

                                            if (Convert.ToInt32(scrollEndObj) > 0)
                                            {
                                                Console.WriteLine($"{profileName} - No more search results, finishing scroll.");
                                                break;
                                            }

                                        }

                                        if (!isCommented)
                                        {
                                            executer.ExecuteScript("window.scrollBy(0, 500000);");

                                        }
                                        else
                                        {
                                            Console.WriteLine($"{profileName} - Commented, check the 'Comments' folder!");
                                            break;
                                        }

                                    }


                                }
                                else
                                {
                                    Console.WriteLine("Can't load to webpage.");
                                }


                            }
                            else
                            {

                                Console.WriteLine(profileName + " not logined to Facebook.");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Can't load to webpage.");
                        }


                    }

                }
                else
                {
                    Console.WriteLine($"You did not input any image to 'Images' folder, please input some images.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{profiles[profileSelection]} - "+ex);
            }



            if (profileSelection + 1 >= profiles.Count)
            {
                Console.WriteLine($"Gonna switch to first profile after {delayBetweenProfiles} minutes.");
            }
            else
            {
                Console.WriteLine($"Gonna switch to {profiles[profileSelection + 1]} after {delayBetweenProfiles} minutes.");
            }


            await Task.Delay(TimeSpan.FromMinutes(delayBetweenProfiles));

            if (profileSelection >= profiles.Count)
            {
                break;
            }



        }


    }


}