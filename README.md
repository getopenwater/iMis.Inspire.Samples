# iMIS Inspire Installation Guide

iMIS Inspire is powered by [OpenWater](https://www.getopenwater.com)

## Installation

Before getting started you should already have your OpenWater instance setup.  This will be something like [demo.imis-inspire.com](#).

1. Download the [iMIS Config Files](https://github.com/getopenwater/iMis.Inspire.Samples/blob/master/InitialConfig/InitialConfig.zip?raw=true)

2. Navigate to Settings > Contacts > Client applications and click Add client application
![image](https://user-images.githubusercontent.com/7950956/80551244-d92b2200-8990-11ea-82d9-d784919faa49.png)

3. Add your client application. Write down your **Client ID** and **Client Secret**
![image](https://user-images.githubusercontent.com/7950956/80551271-f2cc6980-8990-11ea-9b51-bb7cea0691cd.png)



For **Login redirect URL** note your OpenWater instance.  In our example our instance is demo.imis-inspire.com.  

  
The redirect URL will be **https://sso.imis-inspire.com/api/sso/process/** [instance-domain-name]  
  
Thus for our example the final redirect URL will be https://sso.imis-inspire.com/sso/process/demo.imis-inspire.com  
  
4. Next, go to RiSE > Site Builder > Manage Sitemaps > Import
![image](https://user-images.githubusercontent.com/7950956/80551312-11cafb80-8991-11ea-8725-04ff3501db87.png)

5.  Import SiteNavigation.XML
![image](https://user-images.githubusercontent.com/7950956/80551339-25766200-8991-11ea-9fe3-1b09c1304df9.png)

6. For the Content URL use **https://sso.imis-inspire.com/api/sso/admin/** [instance-domain-name]  
  
Once again replace the [instance-domain-name] with your instance name as you did in step 3.  

![image](https://user-images.githubusercontent.com/7950956/80551390-50f94c80-8991-11ea-8ea9-013a7c90b717.png)

7.  Upload InspireIcon.png for the Image URL  

8. Remember to Save and then press Publish
![image](https://user-images.githubusercontent.com/7950956/80551422-6bcbc100-8991-11ea-8286-e34d1d3292bb.png)  

9. Now go to RiSE > Page Builder > Manage Content > Import
![image](https://user-images.githubusercontent.com/7950956/80551483-99186f00-8991-11ea-9a63-0794c79b8809.png)

10. Import PageContent.xml
![image](https://user-images.githubusercontent.com/7950956/80551570-dbda4700-8991-11ea-8102-57c01775a21d.png)

11. You'll see the 3 files, let's start with Login
![image](https://user-images.githubusercontent.com/7950956/80551588-ea286300-8991-11ea-8522-61cf40fd299b.png)


12. Click on Configure and then choose the new Client Application you created in Step 2
![image](https://user-images.githubusercontent.com/7950956/80551617-07f5c800-8992-11ea-8b0b-8b6d2ed7fc1b.png)

![image](https://user-images.githubusercontent.com/7950956/80551623-12b05d00-8992-11ea-8c7d-fed125cd2cf8.png)  

13. Save and Publish then Load Ensure Login and Copy the URL
![image](https://user-images.githubusercontent.com/7950956/80551661-35427600-8992-11ea-801a-25316911fb1c.png)

14. Do the same for Log Out
![image](https://user-images.githubusercontent.com/7950956/80551690-55723500-8992-11ea-8803-0e5327492beb.png)

15. Publish all of the pages then head over to OpenWater.  Go to System Settings > Login Configuration > Allow 3rd Party Corporate Authentication.  Choose iMIS Inspire (Cloud).

![image](https://user-images.githubusercontent.com/7950956/80551710-6753d800-8992-11ea-86dc-7eeb051f8ca8.png)  
  
Then paste in your:  
**Client ID**: from Step 2  
**Client Secret**: from Step 2  
**iMIS Login Page**: from Step 13  
**Token Endpoint**: This is your REST api token endpoint previously known as Asi.Scheduler token end point  
**Login Button Text**: this can be Login or Login with iMIS  
**Single Sign Out Url**: this is the log out URL from step 14  

16. Now let's test it out.  At the top of the screen right click Public Website and load it in an incognito window
![image](https://user-images.githubusercontent.com/7950956/80551720-7044a980-8992-11ea-81ca-dc9be6ef4390.png)  

17. Now click Login.  After logging in you should be brought into the public side of OpenWater already logged in.
![image](https://user-images.githubusercontent.com/7950956/80551733-79ce1180-8992-11ea-8728-ffabdef1bf65.png)


18. Finally go back to your staff page and click the Inspire menu navigation item
![image](https://user-images.githubusercontent.com/7950956/80551756-8d797800-8992-11ea-94c1-a58ab2ef1cf1.png)

If all goes well you should be done with connecting iMIS with OpenWater in under 10 minutes!
