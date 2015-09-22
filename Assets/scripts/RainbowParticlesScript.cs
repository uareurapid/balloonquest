using UnityEngine;
using System.Collections;

public class RainbowParticlesScript : MonoBehaviour {


	public Color[] colors;
	private ParticleSystem ps;
	private ParticleSystem.Particle[] particles;

	// Use this for initialization
	void Start () {

	 ps = GetComponent<ParticleSystem>();
     particles = new ParticleSystem.Particle[ps.maxParticles];
	}

	// Update is called once per frame
	void Update() {
     int count = ps.GetParticles(particles);
     for (int i = 0; i < count; i++) {
         Color c = particles[i].color;
         if (c.r > 0.99 && c.g > 0.99 && c.b > 0.99) {
             particles[i].color = colors[Random.Range(0,colors.Length)];
         }
     }
     ps.SetParticles(particles, count);
 }
}
