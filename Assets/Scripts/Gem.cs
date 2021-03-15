using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
	private static Color SELECTED_COLOR = new Color(0.5f, 0.5f, 0.5f, 1f);
	private static Color UNSELECTED_COLOR = Color.white;
	private bool isSelected;
	private static Gem previousSelected = null;
	private SpriteRenderer spriteRenderer;

	public AudioClip selectSound;
	public AudioClip swapSound;
	public AudioClip clearSound;

	private void Start()
	{
		spriteRenderer = ((Component)this).GetComponent<SpriteRenderer>();
		isSelected = false;
	}

	private void Select()
	{
    isSelected = true;
   spriteRenderer.color = SELECTED_COLOR;
   previousSelected = gameObject.GetComponent<Gem>();
   GetComponent<AudioSource>().PlayOneShot(selectSound);
	}

	private void Unselect()
	{
		isSelected = false;
		spriteRenderer.color = UNSELECTED_COLOR;
		previousSelected = null;
	}

	private void OnMouseDown()
	{
    if (spriteRenderer.sprite == null || GameManager.instance.gameOver) {
      return;
    }
		if (isSelected)
		{
			Unselect();
		}
		else if (previousSelected == null)
		{
			Select();
		}
		else if (isSelectedGemAdjacent())
		{
			SwapGem();
			previousSelected.ClearMatches();
			previousSelected.Unselect();
			ClearMatches();
		}
		else
		{
			((Component)previousSelected).GetComponent<Gem>().Unselect();
			Select();
		}
	}

	private bool isSelectedGemAdjacent()
	{
		Vector2[] adjacentDirections = (Vector2[])(object)new Vector2[4]
		{
			Vector2.up,
			Vector2.down,
			Vector2.left,
			Vector2.right
		};
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < adjacentDirections.Length; i++)
		{
      RaycastHit2D collidedObject = Physics2D.Raycast(transform.position, adjacentDirections[i]);
      if (collidedObject.collider != null)
			{
				list.Add(collidedObject.collider.gameObject);
			}
		}
		if (list.Contains(previousSelected.gameObject))
		{
			return true;
		}
		return false;
	}

	public void SwapGem()
	{
		Sprite sprite = previousSelected.spriteRenderer.sprite;
		previousSelected.spriteRenderer.sprite = spriteRenderer.sprite;
		spriteRenderer.sprite = sprite;
		GetComponent<AudioSource>().PlayOneShot(swapSound);
	}

  private List<GameObject> FindHorizontalMatches() {
     List<GameObject> matchingGems = new List<GameObject>();

     Vector2[] horizontalDirections = new Vector2[] {
       Vector2.left, Vector2.right
     };

     for (int i = 0; i < horizontalDirections.Length; i++) {
       RaycastHit2D collidedObject = Physics2D.Raycast(transform.position, horizontalDirections[i]);
       while (collidedObject.collider != null && collidedObject.collider.GetComponent<SpriteRenderer>().sprite == spriteRenderer.sprite) {
         matchingGems.Add(collidedObject.collider.gameObject);
         collidedObject = Physics2D.Raycast(collidedObject.collider.transform.position, horizontalDirections[i]);
       }
     }

     return matchingGems;
   }

   private List<GameObject> FindVerticalMatches() {
     List<GameObject> matchingGems = new List<GameObject>();

     Vector2[] verticalDirections = new Vector2[] {
       Vector2.up, Vector2.down
     };

     for (int i = 0; i < verticalDirections.Length; i++) {
       RaycastHit2D collidedObject = Physics2D.Raycast(transform.position, verticalDirections[i]);
       while (collidedObject.collider != null && collidedObject.collider.GetComponent<SpriteRenderer>().sprite == spriteRenderer.sprite) {
         matchingGems.Add(collidedObject.collider.gameObject);
         collidedObject = Physics2D.Raycast(collidedObject.collider.transform.position, verticalDirections[i]);
       }
     }

     return matchingGems;
   }

	public void ClearMatches()
	{
		if (spriteRenderer.sprite == null)
		{
			return;
		}
		List<GameObject> list = FindHorizontalMatches();
		List<GameObject> list2 = FindVerticalMatches();
		if (list.Count >= 2)
		{
			spriteRenderer.sprite = null;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].GetComponent<SpriteRenderer>().sprite = null;
			}
		}
		if (list2.Count >= 2)
		{
			spriteRenderer.sprite = null;
			for (int j = 0; j < list2.Count; j++)
			{
				list2[j].GetComponent<SpriteRenderer>().sprite = null;
			}
		}
		if (list.Count >= 2 || list2.Count >= 2)
		{
			GridManager.instance.DropGems();
			GameManager.instance.IncreaseScore();
			((Component)this).GetComponent<AudioSource>().PlayOneShot(clearSound);
		}
	}

	private void Update()
	{
	}
}
