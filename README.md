<p align="center">
  <img src="https://github.com/HiMyNameIsGarch/Password-Manager/blob/main/PassManager-UI/PassManager-UI/PassManager-UI.UWP/Assets/SplashScreen.scale-100.png">
</p>

## General Info
A simple password manager for learning purpose!
This project is divided in 2 parts:
* Web Api
* Actual application

## Technologies
* ASP.NET Web API
* Xamarin Cross-Platform (Only Android and UWP)

## How it works
I've implemented PBKDF2 HMAC-SHA1 with AES-256 bit encryption and salted hashes to increase security for the user. After you put your ShaMan credentials(email + password) it will be generated a unique encryption key to secure your vault. The vault will be encrypted/decrypted at the device level so your data will be secret even for the server. The keys that are used to encrypt/decrypt data are never send to ShaMan and are never accessible by it or anyone else.

## Screenshots
#### You can sign in | see all your items | add a new one | create that type of item
<p align="center">
    <img src="https://github.com/HiMyNameIsGarch/Password-Manager/blob/main/Screenshots/firstScreenshot.png">
</p>

#### You can pick dates | generate strong passwords | see only items of specific type(Passwords) | copy and see fields of an specific item

<p align="center">
    <img src="https://github.com/HiMyNameIsGarch/Password-Manager/blob/main/Screenshots/secondScreenshot.png">
</p>

## Download
Unfortunately, I didn't publish the application, maybe in the future!
