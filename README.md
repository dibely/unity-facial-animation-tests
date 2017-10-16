# unity-facial-animation-tests

A project for testing facial animations in 2d on 3d objects.

## Left and right eye blinking and object tracking

Utilising the the original eye, eyelid and shader assets for Unity found on Ben Jones' blog post (see [Acknowledgements](#Acknowledgements)), the Eye scene demonstrates functionality for blinking the eyes using Unity's animation system as well as manipulating the eye facing direction (UV-coordinates) based on a 3d position.

[![Eye tracking and blinking](https://img.youtube.com/vi/z_aYK4oPfXM/0.jpg)](https://youtu.be/z_aYK4oPfXM)

## Multiple Eyes - Generic blinking

Building on the left and right eye animation the eyes-multiple scene demonstrates using more than 2 eyes with a generic blink eyelid graphic.

[![Eye tracking and blinking](https://img.youtube.com/vi/ub_c08XDKK4/0.jpg)](https://youtu.be/ub_c08XDKK4)

## Mouth animation

The mouth scene demonstrates a very rudimentary way of swapping to different sprites to show emotional expressions.  This is handled by the animation based on a simple value calculated in the mouth script.  In this case it simply swaps between a neutral and happy graphic but other emotional state images could be added as well as intermediate animation steps as required.

## Octopus character with a single eye and mouth

Putting it all together using a test model with a single eye and mouth the octopus-with-eye-and-mouth scene demonstrates how the animations for both the blink and mouth expressions can be combined, along with the eye object tracking.

[![Eye tracking and blinking](https://img.youtube.com/vi/d48o5hHVeNM/0.jpg)](https://youtu.be/d48o5hHVeNM)

## Acknowledgements

Ben Jones for his Zelda facial expression breakdown blog post that can be found [here](http://www.benjones.us/twilight-princess-eyes-breakdown/)