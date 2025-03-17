[System.Serializable]
public class MapDefinition
{
    public int width;
    public int height;
    public int[] spawnCoordinates;
    public int[] exitCoordinates;
    public int[] wallLayer;
    public int[] itemsLayer;
    public int[] ghostPowerUpLayer;


    public void RotateMapClockWise()
    {
        int[,] rotatedWallLayer = RotateLayerClockWise(wallLayer);
        int[,] rotatedObjectsLayer = RotateLayerClockWise(itemsLayer);
        int[,] rotatedGhostPowerUpLayer = RotateLayerClockWise(ghostPowerUpLayer);

        wallLayer = FlattenLayer(rotatedWallLayer);
        itemsLayer = FlattenLayer(rotatedObjectsLayer);
        ghostPowerUpLayer = FlattenLayer(rotatedGhostPowerUpLayer);
    }

    private int[,] RotateLayerClockWise(int[] layer)
    {
        int[,] rotatedLayer = new int[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                rotatedLayer[i, j] = layer[(width - j - 1) * height + i];
            }
        }

        return rotatedLayer;
    }

    private int[] FlattenLayer(int[,] layer)
    {
        int[] flattenedLayer = new int[width * height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                flattenedLayer[i * width + j] = layer[i, j];
            }
        }

        return flattenedLayer;
    }
}
