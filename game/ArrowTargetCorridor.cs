using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTargetCorridor : MonoBehaviour {

	public UnityEventInt SelectArrowIndex = new UnityEventInt();
	public Camera target_camera;
	public SpriteRenderer m_sprArrow;

	private DataCorridorParam m_next;

	public void Initialize(DataCorridorParam _now , DataCorridorParam _next)
	{
		m_next = _next;

		Vector2 image_arrow_dir = new Vector2(0.0f, 1.0f);
		float offset = 2.5f;

		Vector2 dir = new Vector2(_next.master.x - _now.master.x, _next.master.y - _now.master.y);
		Vector3 axis = Vector3.Cross(new Vector3(image_arrow_dir.x, image_arrow_dir.y, 0.0f), new Vector3(dir.x, dir.y, 0.0f));
		//Debug.Log(axis);
		//Debug.Log(Vector2.Angle(image_arrow_dir, dir) * (axis.z < 0 ? -1 : 1));
		float angle = Vector2.Angle(image_arrow_dir, dir) * (axis.z < 0 ? -1 : 1);
		m_sprArrow.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0.0f, 0.0f, 1.0f));

		transform.localPosition = dir.normalized * offset;

	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = new Ray();

			Vector2 worldPoint = target_camera.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			ray = target_camera.ScreenPointToRay(Input.mousePosition);

			//RaycastHit2D ray2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, Mathf.Infinity);
			RaycastHit2D ray2d2 = Physics2D.Raycast((Vector2)worldPoint, (Vector2)ray.direction, Mathf.Infinity);
			//Debug.Log(ray2d2.collider);

			if(ray2d2.collider)
			{
				if (ray2d2.collider.gameObject == gameObject)
				{
					if (m_next != null)
					{
						SelectArrowIndex.Invoke(m_next.index);
					}
					else {
						Debug.Log(ray2d2.collider);
					}
				}
			}
		}
	}


}
