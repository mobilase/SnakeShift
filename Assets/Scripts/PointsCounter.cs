using UnityEngine;
using System.Collections;

public class PointsCounter : MonoBehaviour {
	public float score = 0f;
    public float lives = 3f;
    public float spawnX, spawnY;
    private GameObject apple;
	private GameObject stone;

    void OnTriggerEnter2D(Collider2D col){

		if (col.gameObject.tag == "apple") {
			score++;
			Destroy (col.gameObject);
        }
    }

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "stone") {
			lives--;
			//transform.position.x = transform.position.x - 10;
		}
	}

	void OnGUI(){
		GUI.Box (new Rect (100, 10, 100, 30), "Очки: " + score);
		GUI.Box (new Rect (100, 40, 100, 30), "Жизни: " + lives);
	}
}