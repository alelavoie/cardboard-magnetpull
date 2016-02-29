# MagnetPull - Cardboard Magnet Extended Controls
This simple script extends the magnet controls for the [Cardboard SDK for Unity](https://developers.google.com/cardboard/unity/). 

With MagnetPull, you will now be able to detect when the magnet is pulled or released as well as how long it was pulled.

A demo scene was added so you can easily test everything on your devices.

Note : Not tested on many different devices. Your feedbacks would be greatly appreciated. 

###How does it work, internally
When the Cardboard magnet is pulled or released, the magnitude *and* the angle of the device's compass vector are both affected. This script uses that fact to effectively detect a pull or a release of the magnet.

###Dependencies
[Cardboard SDK for Unity](https://developers.google.com/cardboard/unity/).

###Installation
Attach the MagnetPull.cs script to any object on the scene and you are ready to use it !

###How to use
*Check if magnet is currently pulled:*
```
if (MagnetPull.SDK.isMagnetPulled) {
  //Do stuff
}
```

*Get the time the magnet was pulled:*
```
float pullTime = MagnetPull.SDK.isMagnetPulled;
```

*Listen to a pull event:*
```
if (MagnetPull.SDK.magnetPulled) {
  //Do stuff
}
```

*Listen to a release event:*
```
if (MagnetPull.SDK.magnetReleased) {
  //Do stuff
}
```


###Credits
Thanks to <a href="https://www.assetstore.unity3d.com/en/#!/content/53752" target="_blank">Stagit East</a> for the free and amazing skybox used in the demo. 
