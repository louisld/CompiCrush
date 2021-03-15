// Warning: Some assembly references could not be resolved automatically. This might lead to incorrect decompilation of some parts,
// for ex. property getter/setter access. To get optimal decompilation results, please manually add the missing references to the list of loaded assemblies.
// GridManager
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	public static GridManager instance;

	public int rows;

	public int columns;

	private GameObject[,] gems;

	public List<Sprite> gemSprites = new List<Sprite>();

	public GameObject gem;

	private float gemWidth;

	private float gemHeight;

	private void Start()
	{
		instance = GetComponent<GridManager>();
		Bounds bounds = (gem.GetComponent<SpriteRenderer>()).bounds;
    Vector2 gemDimensions = gem.GetComponent<SpriteRenderer>().bounds.size;
    generateGrid(gemDimensions.x, gemDimensions.y);
	}

	private void generateGrid(float gemWidth, float gemHeight)
	{
		gems = new GameObject[columns, rows];
		this.gemWidth = gemWidth;
		this.gemHeight = gemHeight;
		for (int i = 0; i < columns; i++) {
			for (int j = 0; j < rows; j++) {
				float xPosition = transform.position.x + gemWidth * (float)i;
				float yPosition = transform.position.y + gemHeight * (float)j;
        GameObject newGem = Instantiate(
         gem,
         new Vector3(xPosition, yPosition, 0),
         gem.transform.rotation
         );
				gems[i, j] = newGem;
				newGem.transform.parent = transform;

				List<Sprite> invalidSprites = new List<Sprite>();
				if (i > 0) {
					invalidSprites.Add(gems[i - 1, j].GetComponent<SpriteRenderer>().sprite);
				}
				if (j > 0) {
					invalidSprites.Add(gems[i, j - 1].GetComponent<SpriteRenderer>().sprite);
				}
				newGem.GetComponent<SpriteRenderer>().sprite = RandomSpriteExcluding(invalidSprites);
			}
		}
	}

	private Sprite RandomSpriteExcluding(List<Sprite> sprites)
	{
		List<Sprite> list = new List<Sprite>();
		for (int i = 0; i < gemSprites.Count; i++)
		{
			if (!sprites.Contains(gemSprites[i]))
			{
				list.Add(gemSprites[i]);
			}
		}
		return list[Random.Range(0, list.Count)];
	}

	public List<Vector2Int> GetDroppableGems() {
		List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 0; i < columns; i++) {
			for (int j = 1; j < rows; j++) {
				if (gems[i, j].GetComponent<SpriteRenderer>().sprite != null && (Object)(object)gems[i, j - 1].GetComponent<SpriteRenderer>().sprite == null) {
					list.Add(new Vector2Int(i, j));
					while (j < rows - 1) {
						j++;
						list.Add(new Vector2Int(i, j));
					}
				}
			}
		}
		return list;
	}

	public List<Vector2Int> GetSpawnableGems()
	{
    List<Vector2Int> list = new List<Vector2Int>();
		for (int i = 0; i < columns; i++) {
			for (int j = 1; j < rows; j++) {
				if (gems[i, j].GetComponent<SpriteRenderer>().sprite == (Object)null) {
					list.Add(new Vector2Int(i, j));
				}
			}
		}
		return list;
	}

	public void DropGems() {
		List<Vector2Int> droppableGems = GetDroppableGems();
		while (droppableGems.Count > 0) {
			for (int i = 0; i < droppableGems.Count; i++) {
				Vector2Int gemCoords = droppableGems[i];
				gems[gemCoords.x, gemCoords.y - 1].GetComponent<SpriteRenderer>().sprite = gems[gemCoords.x,gemCoords.y].GetComponent<SpriteRenderer>().sprite;
				gems[gemCoords.x, gemCoords.y].GetComponent<SpriteRenderer>().sprite = null;
			}
			droppableGems = GetDroppableGems();
		}
		List<Vector2Int> spawnableGems = GetSpawnableGems();
		for (int j = 0; j < spawnableGems.Count; j++) {
			Vector2Int sg = spawnableGems[j];
			List<Sprite> list = new List<Sprite>();
			if (sg.x > 0)
			{
				list.Add(gems[sg.x - 1, sg.y].GetComponent<SpriteRenderer>().sprite);
			}
			if (sg.x < columns - 1)
			{
				list.Add(gems[sg.x + 1, sg.y].GetComponent<SpriteRenderer>().sprite);
			}
			if (sg.y > 0)
			{
				list.Add(gems[sg.x, sg.y - 1].GetComponent<SpriteRenderer>().sprite);
			}
			if (sg.y < rows - 1)
			{
				list.Add(gems[sg.x, sg.y + 1].GetComponent<SpriteRenderer>().sprite);
			}
			gems[sg.x, sg.y].GetComponent<SpriteRenderer>().sprite = RandomSpriteExcluding(list);
		}
		for (int k = 0; k < columns; k++) {
			for (int l = 0; l < rows; l++) {
				gems[k, l].GetComponent<Gem>().ClearMatches();
			}
		}
	}

	private void Update()
	{
	}
}
