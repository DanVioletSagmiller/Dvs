# Core
 Core DVS Code
 This is base code I plan to add to multiple projects, what initial project template will be. ALL Unity extras that I don't use are removed to speed up dev/boot time, such as VS Code extentions. But Unity TestRunner is still on. Everything in here is available on an MIT license so you are welcome to download and reuse it with attribution. 
 
 In Addition to the basics this also contains several framework pieces I use by default, like 
 
 ## Observable Locator
 This is available as a UnityPackage to download from the root. https://github.com/DanVioletSagmiller/Dvs/blob/main/Dvs.ObservableLocator.unitypackage
 
 Observable Locator is a combination of Locator Service, which in light terms replaces singletons with a dictionary of instances based on type and Observables, which is a variable that will notify listeners when it changes. The combination means that when these systems/managers change, or become available, that you will be notified. Rather than wieve intricut and complex initialization systems, each system can now just say what systems its waiting for, and when it gets them, it can initialize itself and add itself to the locator. 
 
 ## Events
This is availabble as a UnityPackage to download from the root. https://github.com/DanVioletSagmiller/Dvs/blob/main/Dvs.Events.unitypackage

Events is a very simple event driver for announcing something happened. There are listeners for a type, and a Trigger command for it as well. Since no actual data is passed, you can create interfaces or other types that are completely empty or unused except for this case, allowinng strongly typed names. It is better not to use the ObservableLocator as an event driver, so to stiffle the consideration I'm keeping this system available next to it. These often get used in tandem anyway. 
