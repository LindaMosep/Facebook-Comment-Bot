# Facebook-Comment-Bot
Comment Bot for Facebook sponsored posts.
It's a functional commenting bot for the Facebook.


How to setup?
---
- Setting Facebook Accounts

Create some *[Google Chrome Profiles](https://support.google.com/chrome/answer/2364824?hl=en&co=GENIE.Platform%3DDesktop)* and login to Facebook accounts via using this profiles. 

After that make sure you change [Facebook language](https://www.facebook.com/help/327850733950290) to *English* for all of them accounts.

* Copying Chrome Profiles

Go to `C:\Users\your_username\AppData\Local\Google\Chrome` this path and copy the things in the User Data folder. *(And make sure you closed all the chrome browsers)*

Then Paste this things into `FacebookCommentBot\FacebookCommentBot\bin\Debug\Net6.0\Profiles` this folder. *(You can delete the profiles you did not want, this profiles named '[Profile [Number]](https://www.techentice.com/how-to-find-the-user-folder-for-a-specific-chrome-profile/)')*

+ Config 

Go to `FacebookCommentBot\FacebookCommentBot\bin\Debug\Net6.0\Config.txt`, input the word you wanted the bot search, for example: `SearchWord: art product`

Then set the delay between comments, for example: `MinuteDelayBetweenComments: 1`

Then set the Chrome Visibility *(this is just for testing)*, for example: `ChromeVisible:false` *(false/true, false means it's gonna run browser headless)*

So final config file should be like that:
```
SearchWord: art product
MinuteDelayBetweenComments: 1
ChromeVisible:false
```

After that go to `FacebookCommentBot\FacebookCommentBot\bin\Debug\Net6.0\Texts.txt` and input the texts you wanted bot comment. *(With new line)*

For example:
```
Please check this product!
Wow amazing product!
Incredible wuhuu!
```

After that go to `FacebookCommentBot\FacebookCommentBot\bin\Debug\net6.0\Images` this folder and input the images you wanted to bot attach to comments. *(Images and texts are just selected in order - One image per comment and when images are finish, it goes and selects the first one)*

+ Checking Comments

Go to `FacebookCommentBot\FacebookCommentBot\bin\Debug\net6.0\Comments` this folder and when bot comments, it takes screenshot of this comment automatically and save it in this folder.

