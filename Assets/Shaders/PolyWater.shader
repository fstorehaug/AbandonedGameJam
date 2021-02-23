// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "PolyWater"
{
	Properties
	{
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_DeepColor("Deep Color", Color) = (0,0,0,0)
		_ShalowColor("Shalow Color", Color) = (1,1,1,0)
		_WaveScale("Wave Scale", Vector) = (12,8,0,0)
		_RippleScale("Ripple Scale", Vector) = (60,24,0,0)
		_Depth("Depth", Float) = 49
		_RefractionDistortion("Refraction Distortion", Float) = 0.5
		_RippleHeight("Ripple Height", Range( 0 , 1)) = 0
		_Waveheight("Wave height", Range( 0 , 1)) = 0
		_WaveSpeed("Wave Speed", Range( 0 , 5)) = 0
		_RippleSpeed("Ripple Speed", Range( 0 , 10)) = 0
		_DepthOpacity("Depth Opacity", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.5
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
		#pragma surface surf StandardSpecular alpha:fade keepalpha exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			half3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float4 screenPos;
		};

		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _WaveSpeed;
		uniform float2 _WaveScale;
		uniform float _Waveheight;
		uniform float _RippleHeight;
		uniform float _RippleSpeed;
		uniform float2 _RippleScale;
		uniform float4 _DeepColor;
		uniform float4 _ShalowColor;
		uniform float _RefractionDistortion;
		uniform float _Depth;
		uniform float _DepthOpacity;
		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Smoothness;


		float2 AlignWithGrabTexel( float2 uv )
		{
			#if UNITY_UV_STARTS_AT_TOP
			if (_CameraDepthTexture_TexelSize.y < 0) {
				uv.y = 1 - uv.y;
			}
			#endif
			return (floor(uv * _CameraDepthTexture_TexelSize.zw) + 0.5) * abs(_CameraDepthTexture_TexelSize.xy);
		}


		void ResetAlpha( Input SurfaceIn, SurfaceOutputStandard SurfaceOut, inout fixed4 FinalColor )
		{
			FinalColor.a = 1;
		}


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float GetRefractedDepth111( float3 tangentSpaceNormal, float4 screenPos, inout float2 uv )
		{
			float2 uvOffset = tangentSpaceNormal.xy;
			uvOffset.y *= _CameraDepthTexture_TexelSize.z * abs(_CameraDepthTexture_TexelSize.y);
			uv = AlignWithGrabTexel((screenPos.xy + uvOffset) / screenPos.w);
			float backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
			float surfaceDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screenPos.z);
			float depthDifference = backgroundDepth - surfaceDepth;
			uvOffset *= saturate(depthDifference);
			uv = AlignWithGrabTexel((screenPos.xy + uvOffset) / screenPos.w);
			backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv));
			return depthDifference = backgroundDepth - surfaceDepth;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			half2 temp_cast_0 = (_WaveSpeed).xx;
			float2 uv_TexCoord7 = v.texcoord.xy * _WaveScale;
			half2 panner14 = ( _Time.x * temp_cast_0 + uv_TexCoord7);
			half simplePerlin2D6 = snoise( panner14 );
			half2 temp_cast_1 = (_RippleSpeed).xx;
			float2 uv_TexCoord85 = v.texcoord.xy * _RippleScale;
			half2 panner86 = ( _Time.x * temp_cast_1 + uv_TexCoord85);
			half simplePerlin2D84 = snoise( panner86 );
			half4 appendResult9 = (half4(0.0 , ( ( simplePerlin2D6 * ( _Waveheight / 10.0 ) ) + ( ( _RippleHeight / 10.0 ) * simplePerlin2D84 ) ) , 0.0 , 0.0));
			v.vertex.xyz += appendResult9.xyz;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			half3 ase_worldTangent = WorldNormalVector( i, half3( 1, 0, 0 ) );
			half3 ase_worldBitangent = WorldNormalVector( i, half3( 0, 1, 0 ) );
			half3 ase_worldNormal = WorldNormalVector( i, half3( 0, 0, 1 ) );
			float3 ase_worldPos = i.worldPos;
			half3 normalizeResult10_g10 = normalize( cross( ddy( ase_worldPos ) , ddx( ase_worldPos ) ) );
			o.Normal = mul( float3x3(ase_worldTangent, ase_worldBitangent, ase_worldNormal), normalizeResult10_g10 );
			half3 normalizeResult10_g3 = normalize( cross( ddy( ase_worldPos ) , ddx( ase_worldPos ) ) );
			float3 tangentSpaceNormal111 = ( mul( float3x3(ase_worldTangent, ase_worldBitangent, ase_worldNormal), normalizeResult10_g3 ) * _RefractionDistortion );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 screenPos111 = ase_screenPos;
			float2 uv111 = float2( 0,0 );
			float localGetRefractedDepth111 = GetRefractedDepth111( tangentSpaceNormal111 , screenPos111 , uv111 );
			half temp_output_46_0 = saturate( pow( ( saturate( ( localGetRefractedDepth111 / _Depth ) ) + 1.0 ) , _DepthOpacity ) );
			half4 lerpResult48 = lerp( _DeepColor , _ShalowColor , temp_output_46_0);
			float4 screenColor116 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,uv111);
			half4 lerpResult58 = lerp( lerpResult48 , screenColor116 , temp_output_46_0);
			o.Albedo = lerpResult58.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
1966;16;1753;1017;1944.219;532.0922;1.556463;True;True
Node;AmplifyShaderEditor.CommentaryNode;50;-2451.796,-87.69396;Inherit;False;1148.317;510.7579;Get screen color for refraction and disturbe it with normals;9;55;75;51;110;111;116;106;107;105;Refraction;1,1,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;75;-2402.594,-29.87368;Inherit;False;FacetedNormals;-1;;3;cf4f04a037ae70641ac5d5c2397decb1;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-2426.345,54.83221;Float;False;Property;_RefractionDistortion;Refraction Distortion;7;0;Create;True;0;0;0;False;0;False;0.5;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenPosInputsNode;110;-2218.543,139.3465;Float;False;1;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-2154.582,5.55067;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-1859.537,-25.77695;Float;False;Property;_Depth;Depth;6;0;Create;True;0;0;0;False;0;False;49;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CustomExpressionNode;111;-1982.712,99.79362;Float;False;float2 uvOffset = tangentSpaceNormal.xy@$uvOffset.y *= _CameraDepthTexture_TexelSize.z * abs(_CameraDepthTexture_TexelSize.y)@$uv = AlignWithGrabTexel((screenPos.xy + uvOffset) / screenPos.w)@$$float backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv))@$float surfaceDepth = UNITY_Z_0_FAR_FROM_CLIPSPACE(screenPos.z)@$float depthDifference = backgroundDepth - surfaceDepth@$$uvOffset *= saturate(depthDifference)@$uv = AlignWithGrabTexel((screenPos.xy + uvOffset) / screenPos.w)@$backgroundDepth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv))@$return depthDifference = backgroundDepth - surfaceDepth@;1;False;3;True;tangentSpaceNormal;FLOAT3;0,0,0;In;;Float;False;True;screenPos;FLOAT4;0,0,0,0;In;;Float;False;True;uv;FLOAT2;0,0;InOut;;Float;False;GetRefractedDepth;True;False;0;3;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT2;0,0;False;2;FLOAT;0;FLOAT2;3
Node;AmplifyShaderEditor.CommentaryNode;78;-1757.65,595.133;Inherit;False;1564.49;1134.904;Noise generator for waves;21;88;9;95;92;12;100;6;102;84;101;14;13;93;86;91;85;90;7;16;17;8;Waves;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;88;-1714.086,1300.456;Float;False;Property;_RippleScale;Ripple Scale;5;0;Create;True;0;0;0;False;0;False;60,24;60,24;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;8;-1670.472,664.8029;Float;False;Property;_WaveScale;Wave Scale;4;0;Create;True;0;0;0;False;0;False;12,8;12,8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;81;-1632.908,-697.7291;Inherit;False;1169.201;517.3004;Water color depending on depth;9;39;40;41;42;43;46;47;44;48;Water Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;106;-1667.214,13.0104;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-1500.908,787.863;Float;False;Property;_WaveSpeed;Wave Speed;10;0;Create;True;0;0;0;False;0;False;0;2;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1582.907,-354.4296;Float;False;Constant;_DepthFadePoint;Depth Fade Point;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;16;-1445.997,891.2531;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;7;-1459.735,645.133;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;107;-1528.185,21.78142;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;90;-1472.514,1557.547;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;85;-1487.012,1284.221;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;91;-1527.425,1454.157;Float;False;Property;_RippleSpeed;Ripple Speed;11;0;Create;True;0;0;0;False;0;False;0;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;40;-1340.21,-434.8122;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1347.405,-295.4303;Float;False;Property;_DepthOpacity;Depth Opacity;12;0;Create;True;0;0;0;False;0;False;0;-5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-1020.118,1078.877;Float;False;Constant;_Float0;Float 0;17;0;Create;True;0;0;0;False;0;False;10;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1128.677,998.0434;Float;False;Property;_Waveheight;Wave height;9;0;Create;True;0;0;0;False;0;False;0;0.45;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-1128.825,1163.281;Float;False;Property;_RippleHeight;Ripple Height;8;0;Create;True;0;0;0;False;0;False;0;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;14;-1163.482,765.092;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;86;-1175.371,1284.224;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;84;-943.4001,1278.862;Inherit;True;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;-1405.706,-647.7291;Float;False;Property;_DeepColor;Deep Color;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0.04310166,0.2499982,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;102;-827.1168,1090.877;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;43;-1164.009,-348.4133;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;6;-942.1688,761.4489;Inherit;True;Simplex2D;False;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;100;-822.1168,983.8781;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-679.5425,1169.038;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;47;-859.2112,-584.8116;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;44;-1163.303,-558.5285;Float;False;Property;_ShalowColor;Shalow Color;3;0;Create;True;0;0;0;False;0;False;1,1,1,0;0,0.8088232,0.8088235,0;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;46;-957.7081,-327.2141;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-692.5195,763.0051;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;48;-647.7039,-450.9291;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;95;-510.7749,759.8869;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;117;-1098.968,220.8414;Inherit;False;651.7468;311.061;;3;115;114;113;Needed jank;1,1,1,1;0;0
Node;AmplifyShaderEditor.ScreenColorNode;116;-1532.142,150.7323;Float;False;Global;_GrabScreen0;Grab Screen 0;5;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-347.9275,104.1035;Float;False;Property;_Specular;Specular;0;0;Create;True;0;0;0;False;0;False;0;0.09;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;58;-232.3729,-122.1521;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CustomExpressionNode;115;-734.2203,348.9022;Float;False;FinalColor.a = 1@;7;False;3;True;SurfaceIn;OBJECT;0;In;Input;Float;False;True;SurfaceOut;OBJECT;0;In;SurfaceOutputStandard;Float;False;True;FinalColor;OBJECT;0;InOut;fixed4;Float;False;ResetAlpha;False;True;0;4;0;FLOAT;0;False;1;OBJECT;0;False;2;OBJECT;0;False;3;OBJECT;0;False;2;FLOAT;0;OBJECT;4
Node;AmplifyShaderEditor.Vector4Node;113;-1048.968,296.3011;Float;False;Global;_CameraDepthTexture_TexelSize;_CameraDepthTexture_TexelSize;2;0;Create;True;0;0;0;True;0;False;0,0,0,0;0.001592357,0.001828154,628,547;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;114;-727.4413,270.8412;Float;False;#if UNITY_UV_STARTS_AT_TOP$if (_CameraDepthTexture_TexelSize.y < 0) {$	uv.y = 1 - uv.y@$}$#endif$return (floor(uv * _CameraDepthTexture_TexelSize.zw) + 0.5) * abs(_CameraDepthTexture_TexelSize.xy)@;2;False;1;True;uv;FLOAT2;0,0;In;;Float;False;AlignWithGrabTexel;False;True;0;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;9;-360.1591,746.52;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-344.8577,193.1248;Float;False;Property;_Smoothness;Smoothness;1;0;Create;True;0;0;0;False;0;False;0;0.9;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;76;-281.1263,23.84708;Inherit;False;FacetedNormals;-1;;10;cf4f04a037ae70641ac5d5c2397decb1;0;0;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Half;False;True;-1;3;ASEMaterialInspector;0;0;StandardSpecular;PolyWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;0;-1;0;False;0;0;False;-1;-1;0;False;-1;1;Custom;UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture)@;False;;Custom;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;118;0.4663086,-128.4874;Inherit;False;201;100;Made by func_Mathias;0;;1,1,1,1;0;0
WireConnection;55;0;75;0
WireConnection;55;1;51;0
WireConnection;111;0;55;0
WireConnection;111;1;110;0
WireConnection;106;0;111;0
WireConnection;106;1;105;0
WireConnection;7;0;8;0
WireConnection;107;0;106;0
WireConnection;85;0;88;0
WireConnection;40;0;107;0
WireConnection;40;1;39;0
WireConnection;14;0;7;0
WireConnection;14;2;17;0
WireConnection;14;1;16;1
WireConnection;86;0;85;0
WireConnection;86;2;91;0
WireConnection;86;1;90;1
WireConnection;84;0;86;0
WireConnection;102;0;93;0
WireConnection;102;1;101;0
WireConnection;43;0;40;0
WireConnection;43;1;41;0
WireConnection;6;0;14;0
WireConnection;100;0;13;0
WireConnection;100;1;101;0
WireConnection;92;0;102;0
WireConnection;92;1;84;0
WireConnection;47;0;42;0
WireConnection;46;0;43;0
WireConnection;12;0;6;0
WireConnection;12;1;100;0
WireConnection;48;0;47;0
WireConnection;48;1;44;0
WireConnection;48;2;46;0
WireConnection;95;0;12;0
WireConnection;95;1;92;0
WireConnection;116;0;111;3
WireConnection;58;0;48;0
WireConnection;58;1;116;0
WireConnection;58;2;46;0
WireConnection;9;1;95;0
WireConnection;0;0;58;0
WireConnection;0;1;76;0
WireConnection;0;4;11;0
WireConnection;0;11;9;0
ASEEND*/
//CHKSM=506AD275F50DF3A7830F7FEE417E4F7F4498215C