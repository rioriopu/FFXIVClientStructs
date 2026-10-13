using FFXIVClientStructs.FFXIV.Common.Math;

namespace FFXIVClientStructs.FFXIV.Client.Graphics.Kernel;

// Client::Graphics::Kernel::ImmediateContext

// Client::Graphics::Kernel::ImmediateContextDX11
[GenerateInterop]
[StructLayout(LayoutKind.Explicit, Size = 0x1858)]
public unsafe partial struct ImmediateContext {
    // Offset 0 is ID3D11DeviceContext

    [FieldOffset(0x08)] public Rectangle CurrentScissorRect;

    [Obsolete("Not necessarily the backbuffer, just the current primary render target. Prefer CurrentRenderTargets.")]
    [FieldOffset(0x28)] public Texture* BackBufferReference;
    /// <summary>
    /// The currently bound render targets.
    /// </summary>
    [FieldOffset(0x28)] internal FixedSizeArray5<Pointer<Texture>> _currentRenderTargets;
    /// <summary>
    /// The currently bound depth stencil buffer, if any.
    /// </summary>
    [FieldOffset(0x50)] public Texture* CurrentDepthStencilBuffer;

    [FieldOffset(0xAC)] public uint CurrentDepthState; // The 5 bits of depth state in PackedDepthStencilDesc with the stencil fields masked out
    [FieldOffset(0xB0)] public PackedDepthStencilDesc CurrentStencilState; // With the depth fields masked out
    // 0xC8: InputLayoutDesc

    [FieldOffset(0xD0)] public VertexShader* CurrentVertexShader;
    [FieldOffset(0x1B8)] public PixelShader* CurrentPixelShader;

    // Certain types of complex clearing needs to be done by manually drawing to the render targets & depth buffer.
    [FieldOffset(0x700)] public VertexShader* ManualClearQuadVertexShader;
    [FieldOffset(0x708)] public PixelShader* ManualClearQuadPixelShader;
    // 0x710: ManualClearQuad???
    // 0x718: ManualClearQuadInputLayoutDesc
    [FieldOffset(0x730)] public GeometryShader* CurrentGeometryShader;
    [FieldOffset(0xB20)] public HullShader* CurrentHullShader;

    [FieldOffset(0xBE8), CExporterTypeForce("ID3D11DeviceContext*", true)] public void* D3D11DeviceContext;

    [FieldOffset(0xF10)] public DomainShader* CurrentDomainShader;

    [FieldOffset(0x17B3)] public byte BlendStateFlag; // Might be a bool to force creation of an underlying ID3D11BlendState*
    [FieldOffset(0x17B4)] public PackedBlendStateDesc CurrentBlendState;
    [FieldOffset(0x17B8), CExporterTypeForce("ID3D11DeviceContext*", true)] public void* D3D11DeviceContext_2;

    //[FieldOffset(0x1D70)] public InputLayout* CurrentInputLayout;
    [FieldOffset(0x17D8), CExporterTypeForce("D3D11_PRIMITIVE_TOPOLOGY", true)] public int CurrentPrimitiveTopology;

    [FieldOffset(0x1808)] public VertexShader* CopyVertexShader; // CopyVS.shcd
    [FieldOffset(0x1810)] public PixelShader* CopyPixelShader; // CopyPS.shcd
    [FieldOffset(0x1818)] public PixelShader* CopyValidColorPixelShader; // CopyValidColorPS.shcd
    [FieldOffset(0x1820)] public VertexShader* DepthCopyVertexShader; // DepthCopyVS.shcd
    [FieldOffset(0x1828)] public PixelShader* DepthCopyPixelShader; // DepthCopyPS.shcd

    [MemberFunction("E8 ?? ?? ?? ?? 49 8D 47 58")]
    public partial void SetBlendState(PackedBlendStateDesc blendState);

    [MemberFunction("E8 ?? ?? ?? ?? 41 8B 56 1C 48 8B CE")]
    public partial void SetDepthStencilState(byte obfuscatedDepthState, PackedDepthStencilDesc stencilState);

    // Along with the given packed rasterizer state, the following constant values are also used:
    // FrontCounterClockwise = true
    // DepthClipEnable = true
    // DepthBiasClamp = 100.0f
    // AntialiasedLineEnable = false
    [MemberFunction("E8 ?? ?? ?? ?? 49 8B 46 20 48 8B CE")]
    public partial void SetRasterizerState(PackedRasterizerStateDesc rasterizerState);

    [MemberFunction("E8 ?? ?? ?? ?? 4D 8B 46 68")]
    public partial void SetVertexShader(VertexShader* vertexShader, void** constantBuffers);

    [MemberFunction("E8 ?? ?? ?? ?? 4D 8B 46 78")]
    public partial void SetPixelShader(PixelShader* pixelShader, void** constantBuffers);

    [MemberFunction("E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 8B 57 7C")]
    public partial void SetViewport(IntRectangle* viewportRectangle, float minDepth, float maxDepth);

    [MemberFunction("E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 8B D7 48 8B CB E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 83 7F")]
    public partial void SetMultiViewports(uint viewportCount, IntRectangle* viewports, float* minDepths, float* maxDepths);

    // 旧シグネチャ("E9 ?? ?? ?? ?? 48 8B 4A 10")は 9 バイトしかなく、2026.10.06 では
    // 4 箇所に一致し、しかも 4 つとも別の関数へ解決していた。
    // 呼び出し元経由をやめ、関数本体の先頭を直接指す形にする。
    // 2026.09.01 / 2026.09.15 / 2026.10.06 の 3 版で一意、かつ各版の正しい関数へ
    // 解決することを確認済み。
    [MemberFunction("48 89 5C 24 ?? 57 48 83 EC ?? 48 8B 05 ?? ?? ?? ?? 48 33 C4 48 89 44 24 ?? 8B 15 ?? ?? ?? ?? 48 8B D9 E8")]
    public partial void SetDefaultState();

    [MemberFunction("E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 48 83 7F ?? ?? 0F 84 ?? ?? ?? ?? 48 83 7F")]
    public partial void DoClearCommandViaDraw(RenderCommandClear* clearCommand);

    [MemberFunction("48 89 6C 24 ?? 56 48 83 EC 40 48 83 7A")] // inlined in some places
    public partial void DoClearCommand(RenderCommandClear* clearCommand);

    [MemberFunction("E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? F3 0F 10 5F")]
    public partial void DoSetTargetCommand(RenderCommandSetTarget* command);

    [MemberFunction("E8 ?? ?? ?? ?? 48 8B 4B 30 FF 15")]
    public partial void ProcessCommands(RenderCommandBufferGroup* renderCommands, uint renderCommandCount);

    [MemberFunction("E8 ?? ?? ?? ?? 48 8B 7B 18 45 33 FF")]
    public partial void PreprocessCommands(RenderCommandBufferGroup* renderCommands, uint renderCommandCount);

    [MemberFunction("E8 ?? ?? ?? ?? E9 ?? ?? ?? ?? 0F B6 83 ?? ?? ?? ?? 3C 01 73 70")]
    public partial void ExecuteCommands(uint renderCommandCount, uint* a3, RenderCommandBufferGroup* renderCommands);
}
