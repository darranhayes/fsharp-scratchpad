// https://sharplab.io/
// Explore sharplab memory inspecting features

// Explanation:
// https://coderethinked.com/visualizing-stack-and-heap-with-sharplab-io/

// https://sharplab.io/#v2:EYLgtghglgdgNAGxAMwM5wC4gE4Fd4AmIA1AD4ICmGABGLhhMJdQB7UC81AnD77wLAAoISwB0ASRioADhQDGGABQBKIZJnyMogMoM5Aa2oAyFkKGUaqDNg7UARAAsKCBAHtqAd1fYEBO2qlZBVEACQoIaWorbDNBC1p6RmYCWwBtRQBGOHsnF1c7ZQBuakUAJmy7AHNXVwJgAE8KAuLFAGYKrx8/ZQBdalIAPmoCKAVY9SCtAFkKMG96gHFsCIdjAiA=

let mutable x = 999999999

// x.Inspect()
// Inspect.Stack &x

let str = "hello world"
// Inspect.Heap str

let mutable d = [(1, "hello"); (2, "goodbye"); (3, "world")] |> dict

// Inspect.MemoryGraph &d