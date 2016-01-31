
Puffy Smoke particle emitter

by Alexandre Labedade - alesk@alesk.fr

Puffy Smoke is a particle system with a special render setup which displays a fake volumetric smoke on classic billboard particles,
using a directional light as reference.
Its primary purpose is to emit smoke trails behind moving objects, but it can be used to also make clouds or any other smoke effect.

==============

Disclaimer and terms of usage :

By using this software you accept that you do so at your own risk.
Alexandre Labedade is not liable for any loss or damage of data as a result of use or misuse of this product.
Please remove this software if you are not happy with these conditions.

This product is working with the free version of the great Unity Threading Helper,
more informations here : http://forum.unity3d.com/threads/90128-Unity-Threading-Helper 

==============

History :

22/02/2014 - Version 1.12:

- First iteration of mobile support (iOS & Android) \o/
 IMPORTANT : depending on your mobile hardware (problem found on Android devices) you may have to disable the "Use Threads" option to improve performances.
 I'm trying to figure out what's the problem here...
 
- Many shaders optimizations
- Fake volumetric textures modified to be a square images
- Default internal ambient color set to black

- Materials clean up, now you have 3 materials with different qualities :
  * "PuffySmoke Low" : no details, only the base volumetric effect. This shader is recommended for mobile gpu !
  * "PuffySmoke Medium" : 1 texture allowed for details
  * "PuffySmoke High" : 2 textures allowed for details, the first is displayed at the particle birth, and gradually fade to the second until the particle death.
  
  Note : All shaders are working on mobile gpu, but those with details textures may have poor performances

Changes in the Puffy_Render Script :

- Auto LOD option, exclude some particles from rendering based on distance to camera : 
  * every 4th particle with distance > MaxRenderDistance/4
  * and every 3rd particle with distance > MaxRenderDistance/3
  * and every 2d particle with distance > MaxRenderDistance/2
  
- NearClipping + fading to fix fillrate issues on low gpu :
  * Screensize Near Clipping : value from 0 to 2, is the maximum % of screen space the particle is allowed to fill before starting to fade out (a value of 0 will hide all particles)
  * Near clipping fade range : from 0 to 1, is the range allowed to let the fade occur (also expressed in % of screen space), a value of 0 will pop out particles while 1 will fade them gradually
  

- ForceOnePass removed and replaced by PassMode, now you can specify how the processing is spread over frames :
  * Auto : let the script decided, using the Update threshold value
  * One : will force everything to be done in one frame
  * Multiple : will force every step to be done on multiples frames (3)

- Render checkbox no more needed (disable the script instead)
- Inspector clean up

04/12/2013 - Version 1.11:

- Bug fix and code optimization in the particle emitter for the particles recycling
- Some tweaks in the particles renderer, to improve performances on low hardware
- Particles can now be immortal (set lifeTime to -1)
- The particles age is no longer stored in the color alpha channel, so now the alpha can be modified within the color gradient
- Particles can now have a Luminosity value, the homing missiles demo is updated with this new parameter
- Position variation added to the particle emitter, this allows a random position offset on particles spawning
- Multi threads added for meshes data generation and part of the meshes updates
- New clouds demo added, with a dedicated shader
- Project exported from Unity 4.0.0f7, since it's the minimal version to get it to work

IMPORTANT NOTE: The new clouds stuff is still a work in progress, it will change in the future updates.

25/10/2013 - Versions 1.08 to 1.10:

- Rewrite of the particle emitter process to get a particle per seconds rate, working properly with any TimeScale value
- Code simplification and new emitter classes to inherit from (mesh or multi emitter).
- "Trail mode" removed (no longer needed), and replaced by the "Max gap" value
- New demo scenes.

12/10/2013 - Version 1.07:

- Added "Auto Assign" option on the emitter script, to let it find a renderer
- Added "Assign to renderer name" on the emitter script, to specifiy which renderer must be used if multiple are available.
  If the name is left empty, the first renderer found will be used.

15/09/2013 - Version 1.06:

- Speed improvements : almost x2 on the benchmark scene with 50k particles
- MaxRenderDistance added to the Puffy_Renderer script
- Tweak on the shader to cast raw shadows on other objects (no self shadowing).
- Added the intermediateRatio parameter to the SpawnRow function in the Puffy_Emitter class

07/09/2013 - Version 1.05:

- Speed improvements on the meshes rebuilding and updates (+50% speed for this part)
- Tweaks in the shader on the details animation parameter
- New material + new texture for a first attempt at making a cartoon smoke look (not convincing yet)
- New parameters on the homing missile script (craziness and some random offsets tweaks)

03/09/2013 - Version 1.04:

- Speed improvements : overall refresh rate multiplied by around 3
- Some changes in the smoke color of the missiles demo (end color = start color, instead of white)
- Launch count added to the missile launcher script
- Some corrections in this readme file

27/08/2013 - Version 1.03:

- Tint color as been removed from the shader
- Changes on the shader and lighting options : the smoke now reacts to the light color and intensity,
  and to the scene ambient light color
- Two new parameters in Puffy_Renderer : "Use Ambient color" and "Camera Background Color as Ambient Color"

26/08/2013 - Version 1.02:

- Cleanup in the package (old test files were included)


26/08/2013 - Version 1.01:

- Homing missile demo updated with cleaner code


21/08/2013 - Version 1.0:

- First pupblic release.


==============

How to use :

You need to define one or more emitters and at least one renderer to see something.

- Puffy_Emitter.cs : this script can be assigned on all objects emitting smoke or can be used on a single object as
  a main smoke emitter shared by multiples objects (spawners).
  It only generates particles data and doesn't render anything alone.

Single emitter case : (look at the "Simple Emitter" demo scene)
- Simply add the PuffyEmitter script to your emitter

Mesh emitter case : (look at the "Mesh Emitter" demo scene)
- Add the Puffy_Emitter script and the Puffy_Mesh Spawner script to your mesh emitter
The mesh emitter script will automatically detect its emitter if you doesn't define it manually

Multi emitter case : (look at the Homing missiles Multi Emitter demo scenes )
- Add the Puffy_Emitter script and the Puffy_MultiSpawner script (or your custom class extending it like Demo/Scripts/MissileLauncher.cs)
  to a GameObject
- Use the Puffy_ParticleSpawner script (or your custom class extending it like Demo/Scripts/HomingMissile.cs).
  Then add all the gameObjects using this script in the "Spawner List" of the Puffy_MultiSpawner script in the inspector, or do it
  by scripting with the

CreateSpawner() or MakeSpawner() methods (see MissileLauncher.cs or MultiSpawnerArray.cs for examples)

CreateSpawner() will create a new gameObject and return the instance of the Puffy_ParticleSpawner script attached to it.
MakeSpawner() will use an existing gameObject having a Puffy_ParticleSpawner (or extended) class assigned to it and will also
return the script instance.


- Puffy_Renderer.cs : this script will render the particles generated by the emitters.
It can be added to any gameObject in the scene, this has no real importance.

- The shaders can be found in the shaders list under the "Puffy_Smoke" category
For now, 2 shader are available : "FakedVolumetric" and "FakedVolumetric 2 Details" the second one allows to use 2 details texture,
fading from the birth to the death of the particles.


To manually link an emitter to a renderer, drop the Puffy_Emitter object in the "Emitters" list in the inspector view of
the Puffy_Renderer object.

You can also do it by script :

first identify the Puffy_Renderer script with something like :

Puffy_Renderer theRendererScript = Puffy_Renderer.GetRenderer( "name_of_the_object_holding_the_renderer_script" );

or :
Puffy_Renderer theRendererScript = Puffy_Renderer.GetRenderer( (int)index );

or if you have only one renderer in the scene :
Puffy_Renderer theRendererScript = Puffy_Renderer.GetFirstRenderer();

then :

theRendererScript.AddEmitter(theEmitterScript);


To unlink an emitter from a renderer :

theRendererScript.RemoveEmitter( "name_of_the_object_holding_the_emitter_script" );
theRendererScript.RemoveEmitter( (int)index );
theRendererScript.RemoveEmitter( emitterScript );

==============

Parameters :


Puffy_Emitter

- Auto Assign to renderer : this check box will auto assign the emitter to the first renderer found
- Auto assign to : if the previous check box is activated, you can type the name of the GameObject holding the renderer you want to use.

- Freezed : particles updates will be stopped as long as this option is enabled.
- Auto Emit : the emitter will automatically spawn particles

- Particles/second : how many particles will be emitted each second.

- Sub particles : for all emitted particles, this value will defines how many will be "sub particles" :
a sub particle may have a shorter lifetime value, defined with "Sub lifetime factor".
This helps having a high density of particles while emitting, and get less particles aging... so less particles displayed.
- Sub lifetime factor : value from 0 to 1. 1 means 100% of the defined lifetime.
So you must use a value lower than one to get a benefit from this option
- Debug sub particles : this checkbox will override color parameters to see which particle are normal (magenta) and which are sub (yellow)

- Max gap : to get a continuous trail effect on fast moving objects, you can define a maximum gap distance between particles.
This will override the "particles per seconds" result if the distance between the emitted particles is higher than the defined gap value.
NOTE : if the particles count resulting from this operation is higher than 10 times the number of particles that would have been
produced otherwise, then the max gap distance is ignored and the emitted particles count is switched back to its original value.
This is a hard coded safety, to prevent too much particles to be generated at the same time, I'm still thinking on how to improve it.

- Particles chunk Size : number of particles available in the first place.
- Unlimited particles : if enabled, the particles count of the emitter will be unlimited. As soon as the limit is reached,
  it will be upped by the value of ChunkSize.

- Direction : vector defining the starting direction of the particle
- Direction variation : values randomly added or substracted to the previous parameter.

- Life time : how long will live each particle, in seconds
- Life time variation : value randomly added or substracted to the previous parameter.
  (assume the same explanation for all other 'variation' parameters)

- Start speed : speed of the particle along its direction vector.

- Start size : size of the particle at its birth

- End size : size of the particle at its death

- Color source : how are colored the particles

Three choices are available for now :

BASIC :
- Start color / End Color : color of the particle at its birth and death.

MESH :
- The particles colors will be extracted from the vertices color info, if the mesh has no color info, basic colors will be used instead.

GRADIENT :
- A gradient class (Puffy_Gradient script) will automatically be added to the emitter, to let you define a more subtle gradient over
  the lifetime of the particles
- Gradient Life time : this value represents the life time of the gradient, according to the particles lifetime.
  For example, if the particles are living 5 seconds, and the gradient has a life time of 1, then it will be fully processed
  over the first second of life of the particle, and then the color will stay on the last gradient color key until the death
  of the particle.

The gradient is controlled by another script which will be automatically assigned to the gameObject and automatically enabled/disabled
if the color source is changed later.

- Luminosity : this curve allows to define the light intensity of the smoke (to simulate luminosity),
The curve keys should be in the 0s to 1s time range, as well as values between 0 and 1.
- Luminosity life time : works as the Gradient Life time parameters, and is displayed only when a gradient is not used, else the Gradient Life Time parameter will be used instead


Puffy_MeshSpawner

- Emitter : the emitter script to use, will be automatically detected if attached to the same object
- Particle speed : emission speed of the particles
- Emit Every nth Point : 1 = emit a particle from each vertex of the mesh, 2 = emit every 2 vertices, etc...
- Smooth normals : for meshes with flat faces (like a cube) multiples normals are defined for a single vertex.
So this option will calculate the right average normal for each vertex, it should stay activated.


Puffy_Renderer

- Cores Setup : experimental feature, seems obsolete now, so I may remove it in a future update
- PassMode :
  * Auto : let the script decided, using the Update threshold value
  * One : will force everything to be done in one frame
  * Multiple : will force every step to be done on multiples frames (3)
  
- Debug : display debug informations as an overlay in the game view
- DebugMode : display simple or detailed debug informations

- Render : enable or disable render updates
- Force One Pass : force the rendering to complete in one frame when it could be done over multiple frames
- Update threshold : value used to split the render processing over multiples frames, if it takes too long to process in one frame.
- Light : the directional light used to define the faked volumetric lighting
- Use Ambient color : add the ambient light color defined in Edit / Render Settings to the particles colors
- Camera Background Color as Ambient Color : use the camera background color instead of the ambient color
- Particles material : material used to render all the particles
- Texture col count & Texture row count : value linked to the texture used (how many rows and columns are present on the texture,
  (leave it with 16x4 for now)
- Details scaling : size of the smoke details
- Max Render Distance : particles beyond this distance from the camera will not be rendered
- Emitters : list of the emitters linked to this renderer
- Use Thread : use all your cpu cores to process particles, should stay always ON. 
- Screensize Near Clipping : value from 0 to 2, is the maximum % of screen space the particle is allowed to fill before starting to fade out (a value of 0 will hide all particles)
- Near clipping fade range : from 0 to 1, is the range allowed to let the fade occur (also expressed in % of screen space), a value of 0 will pop out particles while 1 will fade them gradually
- Auto LOD option, exclude some particles from rendering based on distance to camera : 
  * every 4th particle with distance > MaxRenderDistance/4
  * and every 3rd particle with distance > MaxRenderDistance/3
  * and every 2d particle with distance > MaxRenderDistance/2

Shader Options

- Shadow color : color of the shadowed side of the particles
- Particle Texture : use the provided atlas
- Particle Details Texture : use any tiling grayscale noise texture to add details on the smoke
- Fade in : how much the particle will be transparent at birth.
- Opacity : global opacity of the smoke
- Scattering : light transmission through smoke while it's spreading (older particle = wider spread = less shadow color)
- Density : define the amount of small details
- Sharpness : sharpness of the small details
- Details speed : how fast is moving the details texture in particle space


Shader FakedVolumetric Cloud

Dedicated shader for clouds, this is still a work in progress, will change in the future updates

==============

TODO List :

- Improve performances
- Parallel work on a DX11 version whith compute shaders
- Add more textures and demo scenes
