using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class RenderPixelate : CustomPassVolume {
	public ComputeShader PixelateComputeShader;
	[Range(2, 40)] public int BlockSize = 3;

	int _screenWidth;
	int _screenHeight;
	RTHandle _renderTexture;
	Material _pixelateMaterial;

	protected override void Setup(ScriptableRenderContext renderContext, CommandBuffer cmd) {
		CreateRenderTexture();
		_pixelateMaterial = new Material(Shader.Find("RenderPixelate")); // Replace with your pixelate shader

		// Add any setup code for your custom pass here.
	}

	void CreateRenderTexture() {
		_screenWidth = Screen.width;
		_screenHeight = Screen.height;

		_renderTexture = RTHandles.Alloc(_screenWidth, _screenHeight, colorFormat: GraphicsFormat.R8G8B8A8_SRGB, filterMode: FilterMode.Point, enableRandomWrite: true);

		// Set up the render texture parameters as needed.
	}

	protected override void Execute(ScriptableRenderContext renderContext, CommandBuffer cmd, HDCamera camera, CullingResults cullingResult) {
		// Dispatch your compute shader here.
		var mainKernel = PixelateComputeShader.FindKernel("Pixelate");
		cmd.SetComputeIntParam(PixelateComputeShader, "_BlockSize", BlockSize);
		cmd.SetComputeIntParam(PixelateComputeShader, "_ResultWidth", _renderTexture.rt.width);
		cmd.SetComputeIntParam(PixelateComputeShader, "_ResultHeight", _renderTexture.rt.height);
		cmd.SetComputeTextureParam(PixelateComputeShader, mainKernel, "_Result", _renderTexture);

		uint threadGroupsX, threadGroupsY, threadGroupsZ;
		PixelateComputeShader.GetKernelThreadGroupSizes(mainKernel, out threadGroupsX, out threadGroupsY, out threadGroupsZ);

		cmd.DispatchCompute(PixelateComputeShader, mainKernel,
				Mathf.CeilToInt(_renderTexture.rt.width / (float)BlockSize / threadGroupsX),
				Mathf.CeilToInt(_renderTexture.rt.height / (float)BlockSize / threadGroupsY),
				1);

		// Blit the result to the destination.
		HDUtils.DrawFullScreen(cmd, _renderTexture, destination: camera.colorBuffer, material: _pixelateMaterial);

		// If you want to apply additional post-processing, you can do so here.
	}
}
