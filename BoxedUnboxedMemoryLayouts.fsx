// https://sharplab.io/#v2:EYLgtghglgdgNAGxAMwM5wC4gE4Fd4AmIA1AD4D05ABAEID2AHgKYFUEQYRVSpXZPIm/GAGMmvYAE8qABzqwMQ3hBitYBKPxEYodGAFgAUJSoBVGMEYs2HLjyoi9OgOa46uVEaMnzl5qy06bFYAWgcnKFd3T0MMSRkmKgAFDCoAXioAbyMqXKoGECpkBDoOHLzJQuLSjCMAXy9janp/PiZHYKowpggRAAtk1Pt+QWExVj6hJiM4hKoAN3aMIPSs8tygyNhClPW2TSXdGB3awwbDb2arULbRplFxIwQmVLBcTmBnhYAlVcyqTbOWB/fKrACMADoAAwAbio0gyACZoVQ6nCNFodHoQQxwVDoXCEVREfioajUUYAJIwVAJbQQgCyTDAQUkAHFsBAZAMAGTzb6NEwtaxhEZTB4xZ6vd4QT6JeYAFVWAAoqMrIVC4MToQBKLXq0lakm6qg6qk0ukYRnM1kcrm8xWCq6tMIAQWwnOkdGQt3FYl4y0GbQ6BElLyobw+X3mrt+GQA2qQsqCMhrCatkWS0cncanSemkaS6lRSABdc20pbWlnYdmc7lUPmxp20a5dKjuz0An1isbiKiBjC4GTPMPSqPy11KhNJ9XQo26uEG+fE0k6kvlwzUyv0pk1uv2xsxhUt3xtt0eiCSZT8cIwFxuDxFFYybBQSA6RYLCAIXCPQxShGMpygsrqrImVBptqsKQfmq4omWFaWtWtr1g6rqnhYbaBJ0YSOPekSPjEsyJCIizaCsGTZIYeQAm+QIwAAGlUJRlDReSArAACaLE1HsGKHHozFFKxpy0QJ2hHDxIl8WcjSAZGsrRgAwnGyacUx4IEnRWwwFxGbaRJWKaXmhkHJJej6YWKLnNuyF7qhh58qpQA=

// Boxed data is references by pointers and indirection
// Unboxed data is contiguous

// Unboxed record - contiguous
type Pt = {
    x: float
    y: float
}

// Boxed record - each Pt is referenced here
type vector = {
    origin: Pt
    direction: Pt
}

// Boxed - references to Pt records
let mutable vR = { origin = { x = 1.0; y = 2.0 }; direction = { x = 10.0; y = 20.0 } }
// Inspect.MemoryGraph &vR

// Boxed - references to tuples
let mutable vT = ( (1.0, 2.0), (10.0, 20.0) )
// Inspect.MemoryGraph &vT

// Boxed-ish - Contiguous Array of references to Pt records
let mutable vAR = [| { x = 1.0; y = 2.0 }; { x = 10.0; y = 20.0} |]
// Inspect.MemoryGraph &vAR

// Boxed-ish - Contiguous Array of references to tuples
let mutable vAT = [| (1.0, 2.0); (10.0, 20.0) |]
// Inspect.MemoryGraph &vAT

// Unboxed - Contiguous array of floats, no references
let mutable vA = [| 1.0; 2.0; 10.0; 20.0 |]
// Inspect.MemoryGraph &vA

// Unboxed record - Contiguous
type cvector = {
    originX: float
    originY: float
    directionX: float
    directionY: float
}

let mutable vCR = { originX = 1.0; originY = 2.0; directionX = 10.0; directionY = 20.0 }
// Inspect.MemoryGraph &vCR