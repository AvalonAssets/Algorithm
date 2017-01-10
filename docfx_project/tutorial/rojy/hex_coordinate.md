# Hex Coordinate
To use a hex grid, it always involves coordinate calculation. It is very difficult to implement all the functionalities from scratch.

If you are new to hex map, please read an article [here](http://www.redblobgames.com/grids/hexagons/). All the functionalities in the article has been implementation.

> HexCoordinate is immutable.
>

## Coordinate System
*Vola* only supports cube and axial coordinate. Offset coordinate is not supported as there is no benefits using offset coordinate but increase calculation complexity.

No matter you use cube coordinate or axial coordinate to initialize, they returns the same instance.

### Cube Coordinate
It is recommend to use cube coordinate initialization over axial coordinate, even their are interchangeable. Cube coordinate initialization has a validation as `X + Y + Z = 0`.

```csharp
var coordinate = HexCoordinate.FromCube(0, 0, 0);
// Argument Exception: Invalid coordinates
var coordinate = HexCoordinate.FromCube(0, 1, 0);
```

![](~/images/hex1.png)

### Axial Coordinate
Axial coordinate is a simpler representation by using the rules of `X + Y + Z = 0`. Although it is simpler, but it assumes your input is correct as there is no way to verify it. Only use this when you know there is no input error.

The coordinate is define as follow:
* Q = X
* R = Z

```csharp
var coordinate = HexCoordinate.FromAxial(0, 0);
```

![](~/images/hex2.png)

### Direction
To represent direction, `HexCoordinate.Direction` uses alphabet *A* to *F*. *A* is the 90 degree tile from current tile. The rest of the direction follows clockwise. For example, The *A* direction tile of (0, 0, 0) is (1, -1, 0). 

![](~/images/hex3.png)