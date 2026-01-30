function App() {
  return (
    <div className="min-h-screen bg-military-black text-military-green font-mono p-10 flex flex-col items-center justify-center">
      <div className="dorder-2 border-military-green/50 p-8 rounded-lg shadow-[0_0_20px_rgba(16,185,129,0.2)] max-w-2xl w-full">
        <h1 className="text-4xl font-bold tracking-widest uppercase text-center mb-2">
            BabylonianGate
        </h1>

        <div className="flex justify-center mb-8">
          <span className="bg-military-dark px-3 py-1 rounded text-xs border border-military-green/30 animate-pulse text-military-red">
            ● SYSTEM CONNECTED
          </span>
        </div>

        <div className="space-y-4 text-center text-gray-400">
          <p>Connecting to the server...</p>
          <div className="h-1 w-full bg-military-dark rounded overflow-hidden">
            <div className="h-full bg-military-green/50 w-1/3 animate-pulse"></div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default App;