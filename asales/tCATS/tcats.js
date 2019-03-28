{
"plugindir": "x:/",
"requests": {
	"/search/v0":{
		"handlers":[{"req":"mod.dm1", "res":"mod.rt1"}],
		"aggregator":"mod.agg1" },
	"/search/v1":{
		"handlers":[{"req":"mod.dm2", "res":"mod.rt2"}],
		"aggregator":"mod.agg2" }
	}
}