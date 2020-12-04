<?php

namespace App\Http\Requests\Language;

use Illuminate\Foundation\Http\FormRequest;

class StoreRequest extends FormRequest
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
            'name' => 'required',
            'locale' => 'required|min:1|max:5|unique:languages',
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
            'name.required' => __('The name field is required'),
            'locale.required' => __('The code field is required'),
            'locale.min' => __('The code must be at least :max characters', ['min' => 1]),
            'locale.max' => __('The code may not be greater than :max characters', ['max' => 5]),
            'locale.unique' => __('The code has already been taken'),
        ];
    }
}
