import React from 'react'
import { useForm, SubmitHandler } from 'react-hook-form'
import { useNavigate } from 'react-router-dom'
import { TextField, Button } from '@mui/material'
import './style.scss'

import { useAuth } from '@/contexts/Athentication/AuthContext'

interface IFormInput {
    email: string
    password: string
}

const AuthenticationPage: React.FC = () => {
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm<IFormInput>()
    const navigate = useNavigate()
    const { login } = useAuth()

    const onSubmit: SubmitHandler<IFormInput> = async (data) => {
        try {
            await login(data.email, data.password)
            navigate('/')
        } catch (error) {
            console.error('Login failed:', error)
        }
    }

    return (
        <div className="login">
            <form className="login__form" onSubmit={handleSubmit(onSubmit)}>
                <div className="login__form-group">
                    <TextField
                        label="Email address"
                        variant="outlined"
                        fullWidth
                        {...register('email', {
                            required: 'Email is required',
                        })}
                        error={!!errors.email}
                        helperText={errors.email ? errors.email.message : ''}
                    />
                </div>
                <div className="login__form-group">
                    <TextField
                        label="Password"
                        type="password"
                        variant="outlined"
                        fullWidth
                        {...register('password', {
                            required: 'Password is required',
                        })}
                        error={!!errors.password}
                        helperText={
                            errors.password ? errors.password.message : ''
                        }
                    />
                </div>
                <Button
                    type="submit"
                    variant="contained"
                    color="primary"
                    fullWidth
                >
                    Sign in
                </Button>
            </form>
        </div>
    )
}

export default AuthenticationPage
