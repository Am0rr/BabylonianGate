import {Sidebar} from './components/Sidebar';

function App() {
  return (
    <div className = "flex min-h-screen bg-[#050505] text-white fot-mono">
      <Sidebar />

      <main className = "flex-1 p-10 flex flex-col items-center justify-center text-gray-500">
        <div className = "border border-dashed border-gray-700 p-10 rounded-lg text-center">
          <h2 className = "text-2x1 font-bold text-white mb-2">Work in Progress</h2>
          <p>A table of weapons will be available here soon...</p>
        </div>
      </main>
    </div>
  )
}

export default App;