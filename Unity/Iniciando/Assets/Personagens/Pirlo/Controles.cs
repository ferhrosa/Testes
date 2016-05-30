using UnityEngine;
using System.Collections;

namespace Iniciando.Personagens.Pirlo
{
    public class Controles : MonoBehaviour
    {
        [HideInInspector]
        public bool olhandoDireita = true;

		public bool movimentoComFisica = false;

        public float forcaMovimento = 20f;


        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private bool pulando = false;


        void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //animator.SetBool("Andando", true);

			if (!movimentoComFisica)
			{
				var h = Input.GetAxis("Horizontal");

				animator.SetBool("Andando", (h != 0));

				if (h != 0)
				{
					var correndo = Input.GetKey(KeyCode.LeftShift);

					if ((h < 0 && olhandoDireita) || (h > 0 && !olhandoDireita))
						Flip();

					var forca = (correndo ? forcaMovimento * 2 : forcaMovimento);

					var posicao = this.transform.position.x + (forca * h * Time.deltaTime);
					this.transform.position = new Vector3(posicao, this.transform.position.y);
					//rigidbody2D.AddForce(Vector2.right * h * forca);
				}
			}

			if (Input.GetMouseButton(0))
			{
				spriteRenderer.color = Color.red;
			}
			else
			{
				spriteRenderer.color = Color.white;
			}
		}

        void FixedUpdate()
        {
			if (movimentoComFisica) 
			{
	            if (!pulando && Input.GetAxis("Vertical") > 0)
                {
                    pulando = true;
                    this.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 500));
                }
                
                
                var h = Input.GetAxis("Horizontal");

	            var correndo = Input.GetKey(KeyCode.LeftShift);

	            animator.SetBool("Andando", (h != 0));

	            if ((h < 0 && olhandoDireita) || (h > 0 && !olhandoDireita))
	                Flip();

	            //var forca = (correndo ? forcaMovimento * 2 : forcaMovimento);

	            //rigidbody2D.AddForce(Vector2.right * h * forca);

				var velocidade = this.GetComponent<Rigidbody2D>().velocity;
                velocidade.x = h * this.forcaMovimento;

                this.GetComponent<Rigidbody2D>().velocity = velocidade;
			}
        }

        void Flip()
        {
            olhandoDireita = !olhandoDireita;

            var scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
}