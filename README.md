# procedural-mesh-chunk-optimized
This video has a mistake in the algorithm. The cube objects in the switch case statement of DefineMeshData()  should have the ! in front of them to reverse the bool. Currently they do not...  When I define the data for a mesh of cubes, cubes that are on the inside really don't need to have their quads (sides) drawn. No one is going to see them. This tutorial shows a way to figure out how to omit them from the resulting mesh faces.

YouTube:  https://youtu.be/NcgAu2gPBtc
