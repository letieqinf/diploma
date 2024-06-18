import { Routes, Route } from 'react-router-dom'

import Layout from './Layout'
import RequireAuth from './components/RequireAuth'
import PersistLogin from './components/PersistLogin'

import Login from './pages/Login'
import Register from './pages/Register'
import Home from './pages/Home'
import Missing from './pages/Missing'
import Profile from './pages/Profile'

import './App.css'
import ViewUsers from './pages/ViewUsers'

const ROLES = {
    user: 'user',
    student: 'student',
    teacher: 'teacher',
    secretary: 'secretary',
    education: 'education'
}

function App() {
    return (
        <Routes>
            <Route path="/" element={<Layout />}>
                <Route path="login" element={<Login />} />
                <Route path="register" element={<Register />} />
                
                {/* private */}
                <Route element={<PersistLogin />}>
                    <Route element={<RequireAuth allowedRoles={[ROLES.user]} />}>
                        <Route path="/" element={<Home />} />
                        <Route path="*" element={<Missing />} />
                    </Route>

                    <Route element={<RequireAuth allowedRoles={[ROLES.user]}/>}>
                        <Route path="profile" element={<Profile />} />
                    </Route>

                    <Route element={<RequireAuth allowedRoles={[ROLES.education]}/>}>
                        <Route path="tools">
                            <Route path="users" element={<ViewUsers />}/>
                        </Route>
                    </Route>
                </Route>
            </Route>
        </Routes>
    )
}

export default App;
