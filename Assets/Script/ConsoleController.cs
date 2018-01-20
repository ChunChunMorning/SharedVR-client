using UnityEngine;
using System.Collections;

public class ConsoleController : MonoBehaviour
{
	private float m_maxScale;

	private float m_currentScale;

	private NetworkManager m_NetworkManager;

	private Renderer m_renderer;

	private Color[] m_colors;

	[SerializeField]
	private Material[] m_Materials;

	void Awake()
	{
		m_maxScale = m_currentScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3;
		m_NetworkManager = FindObjectOfType<NetworkManager>();
		m_renderer = GetComponent<Renderer>();
		m_colors = new Color[] {
			new Color32(255, 0, 0, 255),
			new Color32(65, 105, 225, 255),
			new Color32(127, 255, 212, 255)
		};
	}

	void Update ()
	{
		m_renderer.material = m_Materials [CardManager.Instance.Founded ()];

		m_renderer.material.color = m_colors[
			!m_NetworkManager.isConnected ? 0 :
			m_NetworkManager.mode == SynchronizeMode.Gaze ? 1 : 2
		];

		m_currentScale = Mathf.Min(m_maxScale, m_currentScale + 0.1f * Time.deltaTime);
		transform.localScale = m_currentScale * Vector3.one;
		transform.Rotate(720 * Mathf.Deg2Rad * Time.deltaTime * Vector3.up);
	}

	public void OnClick()
	{
		m_currentScale = 0.01f;

		if (!m_NetworkManager.Connect())
		{
			SeManager.Instance.Play("bat");

			return;
		}

		SeManager.Instance.Play("correct");
	}
}
