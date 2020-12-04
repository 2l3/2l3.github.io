<?php

namespace App\Http\Requests\Auth;

use Illuminate\Foundation\Http\FormRequest;

class ResetRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    public function authorize()
    {
        return true;
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    public function rules()
    {
        return [
            'token' => 'required|min:60|max:60',
            'password' => 'required|min:6|confirmed',
        ];
    }

    /**
     * Get custom messages for validator errors.
     *
     * @return array
     */
    public function messages()
    {
        return [
            'token.required' => __('Invalid token'),
            'token.min' => __('Invalid token'),
            'token.max' => __('Invalid token'),
            'password.required' => __('The password field is required'),
            'password.min' => __('The password must be at least :min characters', ['min' => 6]),
            'password.confirmed' => __('The password confirmation does not match'),
        ];
    }
}
