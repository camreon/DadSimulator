Shader "FPuddle/Puddle" {
    Properties {
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _MainTex ("Texture", 2D) = "" {}
        _Metallic ("Metallic", Range(0, 1)) = 0
        _Reflection_Power ("Reflection Power", Range(0, 1)) = 0.8
        _Softener ("Softener", Range(0, 1)) = 0
    }
    SubShader {
        Pass {
			Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma target 3.0 //Need to support box projection
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _Metallic;
            uniform float _Reflection_Power;
            uniform float _Softener;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
                float3 tangentDir : TEXCOORD5;
                float3 bitangentDir : TEXCOORD6;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.uv1 = v.texcoord1;
                o.uv2 = v.texcoord2;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( _Object2World, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {

                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float3 normalLocal = _BumpMap_var.rgb;
                float3 normdir = normalize(mul( normalLocal, tangentTransform ));
                float3 refldir = reflect( -viewDirection, normdir );
                float3 ld = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 hdir = normalize(viewDirection+ld);

                float onea = 1;
                float3 oneacolor = 1 * _LightColor0.xyz;
                float gloss = _Reflection_Power;
                float specPow = exp2( gloss * 10.0+1.0);

				//Standart GI
                UnityLight light;
                #ifdef LIGHTMAP_OFF
                    light.color = lightColor;
                    light.dir = ld;
                    light.ndotl = LambertTerm (normdir, light.dir);
                #else
                    light.color = half3(0.f, 0.f, 0.f);
                    light.ndotl = 0.0f;
                    light.dir = half3(0.f, 0.f, 0.f);
                #endif
                UnityGIInput d;
                d.light = light;
                d.worldPos = i.posWorld.xyz;
                d.worldViewDir = viewDirection;
                d.atten = onea;
                d.boxMax[0] = unity_SpecCube0_BoxMax;
                d.boxMin[0] = unity_SpecCube0_BoxMin;
                d.probePosition[0] = unity_SpecCube0_ProbePosition;
                d.probeHDR[0] = unity_SpecCube0_HDR;
                d.boxMax[1] = unity_SpecCube1_BoxMax;
                d.boxMin[1] = unity_SpecCube1_BoxMin;
                d.probePosition[1] = unity_SpecCube1_ProbePosition;
                d.probeHDR[1] = unity_SpecCube1_HDR;
                Unity_GlossyEnvironmentData ugls_en_data;
                ugls_en_data.roughness = 1.0 - gloss;
                ugls_en_data.reflUVW = refldir;
                UnityGI gi = UnityGlobalIllumination(d, 1, normdir, ugls_en_data );
                ld = gi.light.dir;
                lightColor = gi.light.color;
				//end

                float NdotL = max(0, dot( normdir, ld ));
                float LdotH = max(0.0,dot(ld, hdir));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 diffuseColor = _MainTex_var.rgb; // Need this for specular when using metallic
                float specularMonochrome;
                float3 specularColor;
                diffuseColor = DiffuseAndSpecularFromMetallic( diffuseColor, _Metallic, specularColor, specularMonochrome );
                specularMonochrome = 1-specularMonochrome;
                float dot1 = max(0.0,dot( normdir, viewDirection ));
                float dot2 = max(0.0,dot( normdir, hdir ));
                float dot3 = max(0.0,dot( viewDirection, hdir ));
                float visTerm = SmithBeckmannVisibilityTerm( NdotL, dot1, 1.0-gloss );
                float normTerm = max(0.0, NDFBlinnPhongNormalizedTerm(dot2, RoughnessToSpecPower(1.0-gloss)));
                float specularPBL = max(0, (NdotL*visTerm*normTerm) * (UNITY_PI / 4) );
                float3 directSpecular = 1 * pow(max(0,dot(hdir,normdir)),specPow)*specularPBL*lightColor*FresnelTerm(specularColor, LdotH);
                half gt = saturate( gloss + specularMonochrome );
                float3 indirectSpecular = (gi.indirect.specular);
                indirectSpecular *= FresnelLerp (specularColor, gt, dot1);

                float3 specular = (directSpecular + indirectSpecular);
                NdotL = max(0.0,dot( normdir, ld ));
                half fd = 0.5 + 2 * LdotH * LdotH * (1-gloss);
                float3 directDiffuse = ((1 +(fd - 1)*pow((1.00001-NdotL), 5)) * (1 + (fd - 1)*pow((1.00001-dot1), 5)) * NdotL) * oneacolor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += gi.indirect.diffuse;

                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                fixed4 final = fixed4(diffuse + specular,pow((_MainTex_var.a*(1 - refldir).g),_Softener*2));

				return final;
            }
            ENDCG
        }
	}
	Fallback "Diffuse"
}
