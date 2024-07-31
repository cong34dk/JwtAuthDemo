import React, { SyntheticEvent, useState } from "react";
import { useNavigate } from "react-router-dom";

const Register = () => {
    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();

    // Xử lý submit form đăng ký
    const submit = async (e: SyntheticEvent) => {
        e.preventDefault();

        await fetch('https://localhost:7242/api/Auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                name, email, password
            })
        });

        // Chuyển hướng người dùng đến trang đăng nhập sau khi đăng ký thành công
        navigate('/login');
    };

    return (
        <form onSubmit={submit}>
            <h1 className="h3 mb-3 fw-normal">Please register</h1>

            <input className="form-control" placeholder="Name" required
                value={name} onChange={e => setName(e.target.value)}
            />

            <input type="email" className="form-control" placeholder="Email address" required
                value={email} onChange={e => setEmail(e.target.value)}
            />

            <input type="password" className="form-control" placeholder="Password" required
                value={password} onChange={e => setPassword(e.target.value)}
            />

            <button className="btn btn-primary w-100 py-2" type="submit">Register</button>
        </form>
    );
};

export default Register;
