import React, { useState, useEffect } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import Nav from "./components/Nav";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Register from "./pages/Register";

function App() {
    const [name, setName] = useState('');

    // Lấy thông tin người dùng sau khi render App
    useEffect(() => {
        const fetchUser = async () => {
            const response = await fetch('https://localhost:7242/api/Auth/user', {
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
            });

            if (response.ok) {
                const content = await response.json();
                setName(content.name);
            }
        };

        fetchUser();
    }, []);

    return (
        <div className="App">
            <BrowserRouter>
                <Nav name={name} setName={setName} />
                <main className="form-signin w-100 m-auto">
                    <Routes>
                        <Route path="/" element={<Home name={name} />} />
                        <Route path="/login" element={<Login setName={setName} />} />
                        <Route path="/register" element={<Register />} />
                    </Routes>
                </main>
            </BrowserRouter>
        </div>
    );
}

export default App;
