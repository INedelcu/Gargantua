

# Gargantua
## A black hole simulation and rendering project in Unity

### What is a black hole?
###### *The text bellow is heavily inspired from Wikipedia. Some large portions of text including formulas are copy pasted here.
A black hole is a region in space with gravity so intense that nothing, not even light, can escape it, formed from the collapsed core of a massive star or other processes, containing immense mass in a tiny volume, defined by a boundary called the "event horizon" where escape becomes impossible.

In 1916, Albert Einstein publishes his theory of gravity also known as the **general theory of relativity** where the **Einstein field equations** (**EFE**) relate the geometry of spacetime to the distribution of matter within it:

$$R_{\mu \nu} - \frac{1}{2} R g_{\mu \nu} + \Lambda g_{\mu \nu} = \frac{8 \pi G}{c^4} T_{\mu \nu}$$

where:
 - $R_{\mu \nu}$  is the [Ricci curvature tensor](https://en.wikipedia.org/wiki/Ricci_curvature "Ricci curvature")
 - $R$ is the [scalar curvature](https://en.wikipedia.org/wiki/Scalar_curvature)
 - $g_{\mu \nu}$ is the [metric tensor](https://en.wikipedia.org/wiki/Metric_tensor)
 - $\Lambda$ is the [cosmological constant](https://en.wikipedia.org/wiki/Cosmological_constant "Cosmological constant")
 - $T_{\mu \nu}$ is the [stress–energy tensor](https://en.wikipedia.org/wiki/Stress%E2%80%93energy_tensor "Stress–energy tensor")
 
Well, this definitely looks very intimidating. How does this alien-like text translate to actual Unity code?

First, **EFE** is an equation (or system of 10 equations) that describe the [geodesic equation](https://en.wikipedia.org/wiki/Geodesics_in_general_relativity) which dictates how freely falling test objects move through spacetime (e.g. a photon?). Fortunately some smart mathematicians (e.g. [Schwarzschild](https://en.wikipedia.org/wiki/Karl_Schwarzschild) or [Kerr](https://en.wikipedia.org/wiki/Kerr_metric)) found some solutions to the **EFE**.

The **Schwarzschild metric** (a.k.a the **Schwarzschild solution**) is an exact solution to the **EFE** that describes the [gravitational field](https://en.wikipedia.org/wiki/Gravitational_field "Gravitational field") outside a spherical mass, on the assumption that the [electric charge](https://en.wikipedia.org/wiki/Electric_charge "Electric charge") of the mass, [angular momentum](https://en.wikipedia.org/wiki/Angular_momentum "Angular momentum") of the mass, and universal [cosmological constant](https://en.wikipedia.org/wiki/Cosmological_constant "Cosmological constant") are all zero. The solution is a useful approximation for describing slowly rotating astronomical objects such as many stars including Earth and the Sun and also (what's the most important in this project) black holes. 

There are more complex solutions for other types of black holes that take into account the rotation of the black hole and the electric charge(?) for example: [Kerr black holes](https://en.wikipedia.org/wiki/Kerr_metric) which are similar to to Schwarzschild but have rotation, or [Kerr–Nnewman](https://en.wikipedia.org/wiki/Kerr%E2%80%93Newman_metric) that have both rotation and changes. Looking at the equations of particle motion for the rotating black holes, I see a lot of [trigonometric functions](https://en.wikipedia.org/wiki/Kerr%E2%80%93Newman_metric#Equations_of_motion) and I believe it's a good idea to stick witch the simplest one which is the Schwarzschild solution for now.

A **Schwarzschild black hole** or **static black hole** is a  black hole that has neither electric charge nor angular momentum (non-rotating). A Schwarzschild black hole is described by the Schwarzschild metric, and cannot be distinguished from any other Schwarzschild black hole except by its mass.

The Schwarzschild black hole is characterized by a surrounding spherical boundary, called the [event horizon](https://en.wikipedia.org/wiki/Event_horizon "Event horizon"), which is situated at the [Schwarzschild radius](https://en.wikipedia.org/wiki/Schwarzschild_radius "Schwarzschild radius") ($r_{\text{s}}$) often called the radius of a black hole. Any non-rotating and non-charged mass that is smaller than its Schwarzschild radius forms a black hole. Some interesting facts: if the Earth would become a black hole, it would have a radius of 8.9 mm and the Sun would be 3.0 km.

The Schwarzschild metric is a solution of **EFE** in empty space, meaning that it is valid only _outside_ the gravitating body. That is, for a spherical body of radius $R$, the solution is valid for $r>R$.

The Schwarzschild metric is defined in [Schwarzschild coordinates](https://en.wikipedia.org/wiki/Schwarzschild_coordinates) $(t, r, \theta, \phi)$ which are similar to [spherical coordinates](https://en.wikipedia.org/wiki/Spherical_coordinate_system) with the addition of time $t$ since we work now in spacetime:

Spherical coordinate system:
<p align="center">
<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/spherical_coordinates.png?raw=true"  alt="A spherical coordinates"  width="300"  height="300">
</p>

Schwarzschild metric:

$${d s}^{2} = c^2 d \tau^{2} = \left (1 - \frac{r_s}{r} \right) c^2 dt^2 - \left(1-\frac{r_s}{r}\right)^{-1} dr^2 - r^2 {d \Omega}^{2}$$

Where:

 - ${\Omega}^{2}$ is the metric on the two-sphere, i.e. ${\Omega}^{2}=\left(d\theta^{2}+\sin^{2}{\theta}d\phi^2\right)$ Furthermore: 
 - $d\tau^2$ is positive for timelike curves, in which case $\tau$</math> is the [proper time](https://en.wikipedia.org/wiki/Proper_time) (time measured by a clock moving along the same [world line](https://en.wikipedia.org/wiki/World_line) with a test particle)
 - $c$ is the [speed of light](https://en.wikipedia.org/wiki/Speed_of_light)
 -  $t$ is, for $r > r_{s}$, the time coordinate (measured by a clock located infinitely far from the massive body and stationary with respect to it)
 - $r$ is, for $r > r_{s}$, the radial coordinate (measured as the circumference, divided by $2\pi$, of a sphere centered around the massive body)
 - $\Omega$ is a point on the two-sphere $S^2$
 - $\theta$  is the colatitude of $\Omega$ (angle from north, in units of radians) defined after arbitrarily choosing a ''z''-axis
 -  $\phi$ is the longitude of $\Omega$ (also in radians) around the chosen ''z''-axis, and
 - $r_{s}$ is the [Schwarzschild radius](https://en.wikipedia.org/wiki/Schwarzschild_radius "Schwarzschild radius") of the massive body, a scale factor which is related to its mass $M$ by $r_{s} = \frac{2GM}{c^2}$, where $G$ is the [gravitational constant](https://en.wikipedia.org/wiki/Gravitational_constant)

These coordinates are used for the exterior of the black hole only. For both the exterior and interior of the black hole there are other types of coordinates e.g. [Kruskal–Szekeres](https://en.wikipedia.org/wiki/Kruskal%E2%80%93Szekeres_coordinates) but we will not dive into the black hole for this simulation :grin:.

[**Schwarzschild geodesics**](https://en.wikipedia.org/wiki/Schwarzschild_geodesics) describe the motion of test particles in the [gravitational field](https://en.wikipedia.org/wiki/Gravitational_field) of a central fixed mass $M$, that is, motion in the Schwarzschild metric. Photons travel along paths called [null geodesic](https://physics.stackexchange.com/questions/188859/what-is-a-null-geodesic) meaning that $ds^2=0$ where proper time $\tau$ is 0 (the time doesn't "tick" for a photon on its geodesic).

Since the spacetime metric around Schwarzschild black hole is symmetric about $\theta = \frac{\pi}{2}$, any geodesic that begins moving in that plane will remain in that plane indefinitely. Therefore, we orient the coordinate system so that the orbit of the particle lies in that plane and fix the $\theta$ coordinate to be $\frac {\pi }{2}$ ($\,d\theta=0$ and $\sin\theta=1$) so that the metric (of this plane) simplifies to:

$$0=c^2 d \tau^{2} = \left (1 - \frac{r_s}{r} \right) c^2 dt^2 - \left(1-\frac{r_s}{r}\right)^{-1} dr^2 - r^2 d\phi^2$$

We are going to use [Newton notation](https://en.wikipedia.org/wiki/Notation_for_differentiation#Newton's_notation) for differentiation of quantities. Thus, $\dot{r}$ instead of $dr$, which means the derivative of $r$ (position) with respect to $t$ (time) which is simply the velocity. Also, in physics one can use [natural units](https://en.wikipedia.org/wiki/Natural_units) ($G=c=1$) for simplification.

With this in mind, the metric becomes:

$$0=-\left (1 - \frac{2M}{r} \right) \dot t^2 + \left(1-\frac{2M}{r}\right)^{-1} \dot r^2 + r^2 \dot{\phi}^2 [1]$$

There are [2 constants of motion](https://en.wikipedia.org/wiki/Schwarzschild_geodesics#Orbits_of_test_particles) caused by the time independence and symmetry (the black hole doesn't change over time, doesn't move, and is spherically symmetric):
1. Energy ($E$) which is associated with time translation symmetry (the metric components don't depend on time coordinate):

$$\left (1 - \frac{2M}{r} \right) \dot t = E \Rightarrow \dot t=\frac{E}{1-\frac{2M}{r}}[2]$$

2. Angular momentum ($h$) associated with rotational symmetry (the black hole looks the same from every angle):

$$h=\|r \times v\|=r^2 \dot{\phi} \Rightarrow  \dot{\phi}=\frac{h}{r^2}[3]$$

*Here the dot represents differentiation with respect to an affine parameter $\lambda$ since proper time $\tau$ is zero for light (i.e. $\dot t = \frac{dt}{d\lambda}$ and $\dot{\phi} = \frac{d\phi}{d\lambda}$) 

Substituting the formulas for $\dot t$ [1] and $\dot{\phi}$ [2] into the null geodesic equation [1], we get:

$$0=-\left(1-\frac{2M}{r}\right)\left[\frac{E}{1-\frac{2M}{r}}\right]^2 + \left(1-\frac{2M}
{r}\right)^{-1}\dot{r}^{2}+r^{2}\left[\frac{h}{r^2}\right]^2$$

Simplifying some terms, we get:

$$0=-\frac{E^2}{1-\frac{2M}{r}}+\frac{\dot{r}^{2}}{1-\frac{2M}{r}}+\frac{h^2}{r^2}$$

Multiplying the equation by $\left(1-\frac{2M}{r}\right)$ to isolate the energy term, we get:

$$\dot{r}^2+\frac{h^2}{r^2}\left(1-\frac{2M}{r}\right)=E^2[4]$$

Here we have 2 quantities on the left side of the equation:

 1. $\dot{r}$ is the radial velocity
 2. The rest is called the Effective Potential and it depends only on $r$ since $h$  the angular momentum is conserved (constant value):
 
$$V_{eff}(r)=\frac{h^2}{r^2}\left(1-\frac{2M}{r}\right)$$

At this point, we have $\dot{r}^2$ - the square of the radial velocity which depends on the energy $E$ and other stuff (that depends on $r$), but if we can get rid of $E$ (which is a constant) by differentiating again [4], we get the acceleration formula.

Differentiating again with respect to the affine parameter $\lambda$ (time), we get:

$$\frac{d}{d\lambda}(\dot{r}^2) + \frac{d}{d\lambda}V_{eff}(r)=\frac{d}{d\lambda}(E^2)$$

Since $E$ is a constant, the right side of the equation is 0. 

To differentiate $V_{eff}(r)$ we have to use the [chain rule](https://en.wikipedia.org/wiki/Chain_rule) since the function is of variable $r$ but we differentiate with respect to $\lambda$ :

$$\frac{d}{d\lambda}V_{eff}(r)=\frac{dV_{eff}}{dr}\frac{dr}{d\lambda}=\frac{dV_{eff}}{dr} \dot{r}$$

Finally, we get:

$$2\dot{r}\ddot{r}+\frac{dV_{eff}}{dr} \dot{r}=0$$

Dividing both sides by $\dot{r}$ we get the formula for $\ddot{r}$ which is acceleration of the radial coordinate (time derivative of radial velocity $\dot{r}$):

$$\ddot{r}=-\frac{1}{2}\frac{dV_{eff}}{dr}$$

The derivative of $V_{eff}(r)=\frac{h^2}{r^2}-\frac{2Mh^2}{r^3}$ is:

$$\frac{dV_{eff}}{dr}=-2\frac{h^2}{r^3}+3\frac{2Mh^2}{r^4}$$

And $\ddot{r}$ is finally (using $r_{s}=2M$ as [geometrized units](https://en.wikipedia.org/wiki/Geometrized_unit_system)):

$$\ddot{r}=\frac{h^2}{r^3} - \frac{3}{2}\frac{r_{s}h^2}{r^4}[5]$$

$\ddot{r}$ which is the linear acceleration of the radial coordinate $r$ (a scalar) but to actually use it, we would need an acceleration in Cartesian coordinates $\vec{a}=(\ddot{x},\ddot{y}, \ddot{z}).$ Since we fixes $\theta = \frac{\pi}{2}$ earlier because of the rotational symmetry, it means that we can derive the velocity vector in the XY plane and by differentiating it, we get the radial acceleration vector (imagine the photon is attracted to the center of the black hole).

In polar coordinates, the position of a particle is defined by $r$ the radial coordinate and the angle $\phi$ (in the XY plane) which is the rotation around the Z axis.

$$\mathbf{\vec{r}} = r\vec{e_r}$$

- $\vec{e_r}$ is the unit vector pointing outward from the origin
- $\vec{e_\phi}$ is the unit vector pointing in the direction of increasing angle $\phi$ which is perpendicular on $\vec{e_r}$

<p align="center">
<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/spherical_coordinates_xy.png?raw=true"  alt="A spherical coordinates"  width="300"  height="300">
</p>

Because the particle moves, the angle $\phi$ changes with time. This means that $\vec{e_r}$ and $\vec{e_\phi}$ are not constant since they change their direction. In Cartesian coordinates [they are](https://youtu.be/_BuyCwdCxAc?si=XzhAFWh1CEIJUaHd):

$$\vec{e_r}=\vec{x}\cos{\phi} + \vec{y}\sin{\phi}$$ 

and

$$\vec{e_\phi}=-\vec{x}\sin{\phi} + \vec{y}\cos{\phi}$$

Their time derivatives are (using product and chain rule and taking into account that $\phi$ depends on time):

$$\dot{\vec{e_r}}=\frac{d}{dt}\left(\vec{x}\cos{\phi} + \vec{y}\sin{\phi}\right)=\dot{\phi}\left(-\vec{x}\sin{\phi}+\vec{y}\cos{\phi}\right)=\dot{\phi}\vec{e_\phi}$$

and 

$$\dot{\vec{e_\phi}}=\frac{d}{dt}\left(-\vec{x}\sin{\phi} + \vec{y}\cos{\phi}\right)=\dot{\phi}\left(-\vec{x}\cos{\phi}-\vec{y}\sin{\phi}\right)=-\dot{\phi}\left(\vec{x}\cos{\phi}+\vec{y}\sin{\phi}\right)=-\dot{\phi}\vec{e_r}$$

To summarize:

$$\dot{\vec{e_r}}=\dot{\phi}\vec{e_\phi} \ \ \text{and}\ \  \dot{\vec{e_\phi}}=-\dot{\phi}\vec{e_r}$$ 

[To obtain the velocity vector](https://youtu.be/Qu8gp8g5jJ0?t=287), one need to take the derivative of the position vector $\mathbf{\vec{r}}=r\vec{e_r}$:

$$\mathbf{\vec{v}}=\frac{d\mathbf{\vec{r}}}{dt}=\frac{d}{dt}\left(r\vec{e_r}\right)=\dot{r}\vec{e_r} + r\dot{\vec{e_r}}$$

Since $\dot{\vec{e_r}}=\dot{\phi}\vec{e_\phi}$:

$$\mathbf{\vec{v}}=\dot{r}\vec{e_r}+r\dot{\phi}\vec{e_{\phi}}$$

[To obtain the acceleration vector](https://www.youtube.com/watch?v=YA2e4-Bv6Wc), we differentiate $\mathbf{\vec{v}}$ with respect of time:

$$\mathbf{\vec{a}}=\frac{d\mathbf{\vec{v}}}{dt}=\frac{d}{dt}\left(\dot{r}\vec{e_r}+r\dot{\phi}\vec{e_{\phi}}\right)$$

Applying the product rule to both terms:

$$\frac{d}{dt}\left(\dot{r}\vec{e_r}\right)=\ddot{r}\vec{e_r}+\dot{r}\dot{\vec{e_r}} [6]$$

and

$$\frac{d}{dt}\left(r\dot{\phi}\vec{e_{\phi}}\right)=\dot{r}\dot{\phi}\vec{e_\phi}+r\ddot{\phi}\vec{e_\phi}+r\dot{\phi}\dot{\vec{e_\phi}}[7]$$

Combining [5] and [6] we get:

$$\mathbf{\vec{a}}=\left(\ddot{r}\vec{e_r}+\dot{r}\dot{\vec{e_r}}\right) + \left(\dot{r}\dot{\phi}\vec{e_\phi}+r\ddot{\phi}\vec{e_\phi}+r\dot{\phi}\dot{\vec{e_\phi}}\right)$$

Substituting $\dot{\vec{e_r}}=\dot{\phi}$ and $\dot{\vec{e_\phi}}=-\dot{\phi}\vec{e_r}$:

$$\mathbf{\vec{a}}=\left(\ddot{r}\vec{e_r}+\dot{r}\dot{\phi}\vec{e_\phi}\right) + \left(\dot{r}\dot{\phi}\vec{e_\phi}+r\ddot{\phi}\vec{e_\phi}-r\dot{\phi}^2\vec{e_r}\right)$$

Grouping the terms by $\vec{e_r}$ and $\vec{e_\phi}$ we finally get the acceleration vector:

$$\mathbf{\vec{a}}=\left(\ddot{r}-r\dot{\phi^2}\right)\vec{e_r} + \left(r\ddot{\phi}+2\dot{r}\dot{\phi}\right)\vec{e_\phi}$$

So the radial component of the acceleration vector is:

$$a_r=\ddot{r}-r\dot{\phi}^2$$

Note that, since the gravitation of the black hole acts like a central force, it only acts in the radial direction (towards the singularity), thus the acceleration doesn't have a tangential component (the factor of $\vec{e_\phi}$ must be zero).

Combining with [5] with [3]:

$$a_r=\frac{h^2}{r^3} - \frac{3}{2}\frac{r_{s}h^2}{r^4} -r\left(\frac{h^2}{r^4}\right)=-\frac{3}{2}\frac{r_{s}h^2}{r^4}$$

$a_r$ is the magnitude of the radial acceleration vector, and to get a vector, it must be multiplied by $\vec{e_r}=\mathbf{\vec{r}}/r$:

$$\vec{\mathbf{a_r}}=-\frac{3}{2}\frac{r_{s}h^2}{r^5}\mathbf{\vec{r}}$$

This is a function of $r$ (since $h$ is constant) and it can be noticed that points in the opposite direction of $\vec{\mathbf{r}}$ basically toward the center of the black hole.

The HLSL code for this would be:
```
    // These should depend on the Rs actually
    #define kStepSize 0.2
    #define kMaxSteps 1000
    
    float3 GetAcceleration(float3 pos, float h2)
    {
       float r2 = dot(pos, pos);
       float r5 = pow(r2, 2.5);       
       return pos * (-1.5 * kRs * h2 / r5);
    }

    // For computing the path, since the acceleration depends on positon only we can use Stormer-Verlet integration
    // Assume the initial ray direction in world space is called rayDirection
    // The origin is actually the camera position
    // Initial position and velocity: x_0
    float3 pos = CameraPos;
    float3 vel = rayDirection;

    // Precalculate the angular momentum which is constant during integration
    float h = length(cross(pos, vel));
    float h2 = h*h;
    
    float3 prevPos = pos;
    
    // Compute next position: x_1
    pos = prevPos + vel * kStepSize + 0.5f * GetAcceleration(pos, h2) * kStepSize * kStepSize;
    
    // Calculate the trajectory of the photon around the black hole
    // https://en.wikipedia.org/wiki/Verlet_integration#Basic_St%C3%B8rmer%E2%80%93Verlet
    for (int i = 0; i < kMaxSteps; i++)
    {
        // Compute x_(n+1)
        float3 nextPos = 2 * pos - prevPos + GetAcceleration(pos, h2) * kStepSize * kStepSize;
		
        prevPos = pos;
                
        pos = nextPos;
    }
```


## Development progress screenshots:


<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/1.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/2.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/3.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/4.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/5.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/6.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/7.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/8.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/9.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/10.png?raw=true" width="1280">

<img src="https://github.com/INedelcu/Gargantua/blob/main/Images/11.png?raw=true" width="1280">
