Shader "Custom/SpriteMove"
{
	/*
	흔들리는 풀
	풀이나 깃발이 흔들릴 때, Bone을 이용해서 처리하려는 시도는 매우 흔히 볼 수 있는 방법이다. 그렇지만 Bone으로 처리하는 것은
	생각보다 훨씬 작업량이 많고 매우 무겁다. 특정하고 정확한 애니메이션이 필요하지 않는다면 쉐이더로 처리하는게 휠씬 좋음
	*/
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Cutoff("Cutoff",float)=0.5
		_Move("Move",Range(0,0.5))=0.1
		_Timeing("Timing",Range(0,5))=1
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue" = "AlphaTest" }

        CGPROGRAM
		/*
		그림자 패스는 현제 우리가 작성한 패스가 아닌 다른 패스에서 작성되고 있다. 그것을 우리가 직접
		제어하지는 못하고 있다. addshadow 구문을 사용하면,우리가 버택스에 적용한 애니메이션 데이터를
		그림자 패스에 전달해준다
		*/
	   #pragma surface surf Lambert alphatest:_Cutoff vertex:vert addshadow

        sampler2D _MainTex;
		float _Move;
		float _Timeing;

		void vert(inout appdata_full v)
		{
			
			//풀의 전체가 위/아래로 흔들린다. 거기다가 그림자는 움직이지 않게 되면서, 그리맞가 풀 위로 뚫고 올라오는 사태가 일어난다
			//v.vertex.y +=sin(_Time.y*0.1);
			//버텍스 컬러의 R 채널을 기반으로 흔들리는 강도를 조절
			v.vertex.y += sin(_Time.y*_Timeing)*_Move*v.color.r;
		}
        struct Input
        {
            float2 uv_MainTex;
			float4 color:COLOR;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
	FallBack "Legacy Shaders/Traparent/Cutout/VertexLit"
}
