import {Sidebar} from './components/Sidebar';
import { Header } from './components/Header';

function App() {
  return (
    <div className = "flex min-h-screen bg-[#050505] text-white fot-mono">
      <Sidebar />
      <div className="flex-1 flex flex-col relative overflow-hidden">
        <Header />
        <main className="flex-1 overflow-y-auto p-8">
            <div className="mt-8 border border-dashed border-white/10 rounded h-96 flex items-center justify-center text-gray-600 uppercase tracking-widest text-sm">
                Inventory Table Area
            </div>
        </main>
      </div>
    </div>
  )
}

export default App;