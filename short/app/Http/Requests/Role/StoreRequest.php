<?php

namespace App\Http\Requests\Role;

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
            'name' => 'required|max:255|unique:roles,name',
            'permissions' => 'required|array|min:1',
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
            'name.max' => __('The name may not be greater than :max characters', ['max' => 255]),
            'name.unique' => __('The name has already been taken'),
            'permissions.required' => __('Select at least one permission'),
            'permissions.min' => __('Select at least one permission'),
        ];
    }

    /**
     * Prepare the data for validation.
     *
     * @return void
     */
    protected function prepareForValidation()
    {
        $permissions = [];
        foreach ($this->get('permissions') as $key => $value) {
            if ($value) {
                $permissions[] = $key;
            }
        }
        $this->merge([
            'permissions' => $permissions,
        ]);
    }
}
