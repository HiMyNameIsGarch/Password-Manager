<p align="center">
  <img style="display: block; margin-left: auto; margin-right: auto" src="https://github.com/HiMyNameIsGarch/Password-Manager/blob/main/PassManager-UI/PassManager-UI/PassManager-UI.UWP/Assets/SplashScreen.scale-100.png">
</p>

## General Info
A simple password manager for learning purpose!
This project is divided in 2 parts:
* Web Api
* Actual application

## Technologies
* ASP.NET Web API
* Xamarin Cross-Platform (Only Android and UWP)

## Screenshots and Gifs

## How it works
I've implemented PBKDF2 HMAC-SHA1 with AES-256 bit encryption and salted hashes to increase security for the user. After you put your ShaMan credentials(email + password) it will be generated a unique encryption key to secure your vault. The vault will be encrypted/decrypted at the device level so your data will be secret even for the server. The keys that are used to encrypt/decrypt data are never send to ShaMan and are never accessible by it.


## Download
You can download the application <a href="https://google.com" target="_blank">here<a/> for Android and <a href="https://google.com" target="_blank">here<a/> for Windows, this is not supported on IOS!
